using System.Media;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using NAudio.Wave;

namespace DeSicaBar;

sealed class AppDelegate : IDisposable
{
    record BuiltInClip(string Label, string Base, uint VKey, string Display);

    static readonly BuiltInClip[] BuiltIn =
    [
        new("A Dragon Ball",                  "a-dragon-ball",                  0x31, "Ctrl+Alt+1"),
        new("Aggiudicato",                    "aggiudicato",                    0x32, "Ctrl+Alt+2"),
        new("Ansia terribile",                "ansia-terribile",                0x33, "Ctrl+Alt+3"),
        new("Category",                       "category",                       0x34, "Ctrl+Alt+4"),
        new("Delicatissimi",                  "delicatissimi",                  0x35, "Ctrl+Alt+5"),
        new("Fucilata all'incrocio",          "fucilata-incrocio",              0x36, "Ctrl+Alt+6"),
        new("Ma chi so' Mission Impossible",  "ma-chi-so-mission-impossible",   0x37, "Ctrl+Alt+7"),
        new("Ma vattene a fa'",               "ma-vattene-a-fa",                0x38, "Ctrl+Alt+8"),
        new("Na bella figura de merda",       "na-bella-figura-de-merda",       0x39, "Ctrl+Alt+9"),
        new("Scherzo innocente, una burla",   "scherzo-innocente-una-burla",    0x30, "Ctrl+Alt+0"),
        new("Scivolata sul burino",           "scivolata-sul-burino",           0xE2, "Ctrl+Alt+<"),
        new("Sono un troione",                "sono-un-troione",                0xDB, "Ctrl+Alt+'"),
        new("Stendere un velo",               "stendere-velo",                  0xDD, "Ctrl+Alt+ì"),
        new("Sto a scherzà, sto a scherzà",   "sto-a-scherza-sto-a-scherza",   0xBB, "Ctrl+Alt++"),
    ];

    static readonly string[] AudioExts = ["mp3", "mp4", "m4a", "wav", "aiff", "aif"];
    const string RegKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
    const string RegValue = "DeSicaBar";
    const double MaxDurationSec = 30.0;

    readonly NotifyIcon tray;
    readonly HotKeyManager hotKeys;
    WaveOutEvent? waveOut;
    AudioFileReader? reader;
    string? currentBase;
    System.Threading.Timer? stopTimer;

    string CustomDir
    {
        get
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "DeSicaBar", "custom");
            Directory.CreateDirectory(dir);
            return dir;
        }
    }

    static string ExeDir => Path.GetDirectoryName(Environment.ProcessPath) ?? AppContext.BaseDirectory;

    public AppDelegate()
    {
        tray = new NotifyIcon { Visible = true, Text = "De Sica Bar" };

        // Carica icona: cerca app.ico accanto all'exe, poi usa icona di fallback
        var icoPath = Path.Combine(ExeDir, "panettone.ico");
        if (File.Exists(icoPath))
            tray.Icon = new Icon(icoPath);
        else
            tray.Icon = SystemIcons.Application;

        tray.MouseClick += (_, e) =>
        {
            if (e.Button == MouseButtons.Left)
                tray.ContextMenuStrip?.Show(Cursor.Position);
        };

        hotKeys = new HotKeyManager();
        hotKeys.HotKeyPressed += OnHotKey;

        for (int i = 0; i < BuiltIn.Length; i++)
            hotKeys.Register(i, BuiltIn[i].VKey);

        BuildMenu();
    }

    void BuildMenu()
    {
        var menu = new ContextMenuStrip();

        foreach (var (clip, i) in BuiltIn.Select((c, i) => (c, i)))
        {
            var item = new ToolStripMenuItem($"{clip.Label}\t{clip.Display}");
            var idx = i;
            item.Click += (_, _) => PlayBuiltIn(BuiltIn[idx].Base);
            menu.Items.Add(item);
        }

        var customs = LoadCustomClips();
        if (customs.Length > 0)
        {
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ToolStripMenuItem("Suoni personalizzati") { Enabled = false });
            foreach (var path in customs)
            {
                var label = Path.GetFileNameWithoutExtension(path);
                var p = path;
                var item = new ToolStripMenuItem(label);
                item.Click += (_, _) => PlayFile(p);
                menu.Items.Add(item);
            }
        }

        menu.Items.Add(new ToolStripSeparator());

        var add = new ToolStripMenuItem("Aggiungi suono personalizzato…");
        add.Click += (_, _) => AddCustomSound();
        menu.Items.Add(add);

        var openDir = new ToolStripMenuItem("Apri cartella suoni");
        openDir.Click += (_, _) => OpenCustomDir();
        menu.Items.Add(openDir);

        menu.Items.Add(new ToolStripSeparator());

        var startup = new ToolStripMenuItem("Avvia con Windows")
        {
            Checked = IsStartupEnabled()
        };
        startup.Click += (_, _) =>
        {
            ToggleStartup();
            startup.Checked = IsStartupEnabled();
        };
        menu.Items.Add(startup);

        var about = new ToolStripMenuItem("Informazioni e disclaimer…");
        about.Click += (_, _) => ShowAbout();
        menu.Items.Add(about);

        menu.Items.Add(new ToolStripSeparator());

        var quit = new ToolStripMenuItem("Esci");
        quit.Click += (_, _) => Application.Exit();
        menu.Items.Add(quit);

        tray.ContextMenuStrip = menu;
    }

    string[] LoadCustomClips() =>
        Directory.GetFiles(CustomDir)
            .Where(f => AudioExts.Contains(Path.GetExtension(f).TrimStart('.').ToLower()))
            .OrderBy(f => Path.GetFileName(f).ToLower())
            .ToArray();

    // MARK: - Playback

    void OnHotKey(int id)
    {
        if (id >= 0 && id < BuiltIn.Length)
            PlayBuiltIn(BuiltIn[id].Base);
    }

    void PlayBuiltIn(string baseName)
    {
        foreach (var ext in AudioExts)
        {
            var path = Path.Combine(ExeDir, "clips", $"{baseName}.{ext}");
            if (File.Exists(path)) { PlayFile(path, baseName); return; }
        }
    }

    void PlayFile(string path, string? baseName = null)
    {
        baseName ??= Path.GetFileNameWithoutExtension(path);

        // Toggle: stesso clip in riproduzione → stop
        if (currentBase == baseName && waveOut?.PlaybackState == PlaybackState.Playing)
        {
            StopPlayback();
            return;
        }

        StopPlayback();

        try
        {
            reader = new AudioFileReader(path);
            waveOut = new WaveOutEvent();
            waveOut.Init(reader);
            waveOut.PlaybackStopped += (_, _) => StopPlayback();
            currentBase = baseName;
            waveOut.Play();

            stopTimer = new System.Threading.Timer(_ => StopPlayback(),
                null, TimeSpan.FromSeconds(MaxDurationSec), System.Threading.Timeout.InfiniteTimeSpan);
        }
        catch
        {
            StopPlayback();
        }
    }

    void StopPlayback()
    {
        stopTimer?.Dispose();
        stopTimer = null;
        waveOut?.Stop();
        waveOut?.Dispose();
        waveOut = null;
        reader?.Dispose();
        reader = null;
        currentBase = null;
    }

    // MARK: - Custom sounds

    // Le tray app non hanno una finestra madre: senza owner l'OpenFileDialog
    // appare dietro le altre finestre. Creiamo una Form invisibile topmost
    // solo per fare da parent e poi la chiudiamo subito.
    static Form CreateDialogOwner()
    {
        var form = new Form
        {
            ShowInTaskbar = false,
            FormBorderStyle = FormBorderStyle.None,
            Opacity = 0,
            Size = new Size(1, 1),
            StartPosition = FormStartPosition.CenterScreen,
            TopMost = true
        };
        form.Show();
        return form;
    }

    void AddCustomSound()
    {
        using var owner = CreateDialogOwner();
        using var dlg = new OpenFileDialog
        {
            Title = "Scegli un file audio",
            Filter = "File audio|*.mp3;*.mp4;*.m4a;*.wav;*.aiff;*.aif|Tutti i file|*.*",
            Multiselect = false
        };
        var result = dlg.ShowDialog(owner);
        if (result != DialogResult.OK) return;

        var dst = Path.Combine(CustomDir, Path.GetFileName(dlg.FileName));
        if (File.Exists(dst)) File.Delete(dst);
        File.Copy(dlg.FileName, dst);
        BuildMenu();
    }

    void OpenCustomDir()
    {
        System.Diagnostics.Process.Start("explorer.exe", CustomDir);
    }

    // MARK: - Startup

    bool IsStartupEnabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RegKey);
        return key?.GetValue(RegValue) is string v && v == Environment.ProcessPath;
    }

    void ToggleStartup()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RegKey, writable: true);
        if (key is null) return;
        if (IsStartupEnabled())
            key.DeleteValue(RegValue, throwOnMissingValue: false);
        else
            key.SetValue(RegValue, Environment.ProcessPath ?? string.Empty);
    }

    // MARK: - About

    void ShowAbout()
    {
        MessageBox.Show(
            """
            De Sica Bar — Fan project non commerciale
            Versione 1.0 (Windows) · Fork by ale.ciano91@gmail.com

            Basato su Boris Bar by Andrea Ricciotti / PunxCode — grazie!

            De Sica Bar è un progetto amatoriale, gratuito, open source,
            senza scopo di lucro, dedicato a Cristian De Sica.

            NON AFFILIAZIONE. Non è un prodotto ufficiale.
            Non è affiliato, sponsorizzato o approvato da Filmauro,
            Medusa, Mediaset, né dagli autori o dagli interpreti.

            ORIGINE AUDIO. I clip sono brevi estratti scaricati da YouTube,
            usati a solo scopo di omaggio, critica, commento, satira e pastiche.

            NESSUN LUCRO. Nessuna vendita, donazione, pubblicità, tracking.

            DMCA / takedown: ale.ciano91@gmail.com
            Codice: licenza MIT.
            """,
            "De Sica Bar",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    public void Dispose()
    {
        StopPlayback();
        hotKeys.Dispose();
        tray.Visible = false;
        tray.Dispose();
    }
}
