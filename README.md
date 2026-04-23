# 🎄 De Sica Bar

> ⚠️ **FAN PROJECT NON COMMERCIALE — NON-COMMERCIAL FAN PROJECT**  
> Questo è un progetto amatoriale gratuito, open source, senza scopo di lucro né valore commerciale, creato da un fan per uso personale e di altri fan. Non è affiliato, sponsorizzato, approvato o in alcun modo connesso con Filmauro, Medusa, Mediaset, o con gli autori, registi, interpreti o detentori dei diritti dei film. Tutti i marchi, titoli, personaggi, dialoghi e opere derivate sono proprietà dei rispettivi titolari.  
> I clip audio usati a scopo dimostrativo sono stati scaricati da YouTube (contenuti pubblicamente accessibili caricati da terzi) e usati qui esclusivamente per scopo illustrativo, satirico e di omaggio. **Nessun ricavo, donazione, pubblicità o monetizzazione è associato a questo progetto.**  
> Se sei il titolare dei diritti e desideri la rimozione dei contenuti, scrivi a **ale.ciano91@gmail.com** o apri una [issue](../../issues): i file verranno rimossi tempestivamente, entro 24 ore, senza discussione.

---

App ispirata alle frasi di Cristian De Sica. 13 clip audio iconici a portata di shortcut globale, disponibile per **macOS** e **Windows**.

Ultra-leggera, zero dipendenze, nativa su entrambe le piattaforme.

---

## 🖥 Piattaforme

| | macOS | Windows |
|---|---|---|
| **Posizione icona** | Menu bar (in alto) | System tray (in basso a destra) |
| **Shortcut** | `⌥⌘1` … `⌥⌘[` | `Ctrl+Alt+1` … `Ctrl+Alt+[` |
| **Avvio automatico** | Avvia al login (SMAppService) | Avvia con Windows (Registry) |
| **Suoni custom** | `~/Library/Application Support/DeSicaBar/custom/` | `%APPDATA%\DeSicaBar\custom\` |
| **Formato release** | `.dmg` | `.zip` con `.exe` |
| **Requisiti** | macOS 13 Ventura+ | Windows 10/11 x64 |
| **Runtime** | Nativo Swift | .NET 8 incluso nell'exe |
| **Peso binario** | ~100 KB | ~15 MB (self-contained) |
| **Stack** | Swift + Cocoa + AVFoundation | C# + WinForms + NAudio |

---

## ✨ Features (entrambe le versioni)

- 🎄 Icona panettone nella menu bar / system tray (invisibile nel Dock / nella taskbar)
- 🎧 13 clip audio con shortcut globali
- ➕ Carica i tuoi suoni personalizzati via file picker
- ⏱ Durata max 30s per clip, toggle start/stop premendo di nuovo lo shortcut
- 🚀 Avvio automatico con il sistema (opzionale, togglable dal menu)
- 🪶 Nessun Electron, nessun Python

---

## 📦 Installazione — macOS

### Opzione A — DMG (consigliata)

1. Scarica `DeSicaBar-*.dmg` dall'ultima [Release](../../releases/latest)
2. Aprilo e trascina `DeSicaBar.app` nella cartella `Applications`
3. **Primo avvio — rimuovere la quarantena:** l'app non è firmata Apple Developer ID, quindi macOS la marca come "danneggiata". Apri Terminale e lancia una volta:
   ```bash
   xattr -cr /Applications/DeSicaBar.app
   ```
   Poi apri l'app normalmente col doppio click.  
   (Alternativa: tasto destro → **Apri** → **Apri** — funziona solo su versioni vecchie di macOS, su Sequoia / Tahoe serve il comando sopra.)
4. Clicca il panettone nella menu bar → scegli un clip.

### Opzione B — Build dai sorgenti

```bash
git clone https://github.com/w-skoll/desica-bar.git
cd desica-bar
./build.sh
open DeSicaBar.app
```

Richiede macOS 13+ e Command Line Tools (`xcode-select --install`).

---

## 📦 Installazione — Windows

### Opzione A — ZIP (consigliata)

1. Scarica `DeSicaBar-*-win.zip` dall'ultima [Release](../../releases/latest)
2. Estrai in una cartella a piacere (es. `C:\Program Files\DeSicaBar`)
3. Avvia `DeSicaBar.exe`
4. L'icona panettone appare nella system tray (angolo in basso a destra della taskbar)

> ⚠️ **Primo avvio — SmartScreen:** Windows potrebbe mostrare "Windows ha protetto il PC".  
> Clicca **"Ulteriori informazioni"** → **"Esegui comunque"**.  
> Questo avviene perché l'app non ha una firma digitale a pagamento — è normale per i progetti open source gratuiti.

### Opzione B — Build dai sorgenti

```powershell
git clone https://github.com/w-skoll/desica-bar.git
cd desica-bar/windows
dotnet run
```

Richiede [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

---

## 🎵 Aggiungere suoni personalizzati

Dal menu dell'app (uguale su entrambe le piattaforme):

- **"Aggiungi suono personalizzato…"** → file picker → il suono viene copiato nella cartella custom e compare nel menu
- **"Apri cartella suoni"** → per rinominare/cancellare manualmente

Cartelle:
- **macOS:** `~/Library/Application Support/DeSicaBar/custom/`
- **Windows:** `%APPDATA%\DeSicaBar\custom\`

Formati supportati: `mp3`, `mp4`, `m4a`, `wav`, `aiff`.  
Nome file = label nel menu.

---

## ⌨️ Shortcut globali predefiniti

Funzionano ovunque, anche con altre app in primo piano. Ripremere lo stesso shortcut ferma la riproduzione.

| Clip | macOS | Windows |
|------|-------|---------|
| A Dragon Ball | `⌥⌘1` | `Ctrl+Alt+1` |
| Aggiudicato | `⌥⌘2` | `Ctrl+Alt+2` |
| Ansia terribile | `⌥⌘3` | `Ctrl+Alt+3` |
| Category | `⌥⌘4` | `Ctrl+Alt+4` |
| Delicatissimi | `⌥⌘5` | `Ctrl+Alt+5` |
| Fucilata all'incrocio | `⌥⌘6` | `Ctrl+Alt+6` |
| Ma chi so' Mission Impossible | `⌥⌘7` | `Ctrl+Alt+7` |
| Ma vattene a fa' | `⌥⌘8` | `Ctrl+Alt+8` |
| Na bella figura de merda | `⌥⌘9` | `Ctrl+Alt+9` |
| Scherzo innocente, una burla | `⌥⌘0` | `Ctrl+Alt+0` |
| Sono un troione | `⌥⌘-` | `Ctrl+Alt+-` |
| Stendere un velo | `⌥⌘=` | `Ctrl+Alt+=` |
| Sto a scherzà, sto a scherzà | `⌥⌘[` | `Ctrl+Alt+[` |

---

## 🛠 Stack tecnico

### macOS
- **Swift** (single-file `desica_bar.swift`, ~200 righe)
- **Cocoa** — NSStatusItem, NSMenu
- **AVFoundation** — AVAudioPlayer
- **ServiceManagement** — SMAppService per login item (macOS 13+)
- **Carbon.HIToolbox** — RegisterEventHotKey per shortcut globali (zero permessi accessibility)

### Windows
- **C#** (cartella `windows/`, ~300 righe)
- **WinForms** — NotifyIcon, ContextMenuStrip
- **NAudio** — riproduzione MP3/WAV
- **Win32 P/Invoke** — RegisterHotKey per shortcut globali
- **Registry** — HKCU\...\Run per avvio con Windows

---

## 📜 Licenza e disclaimer

### Codice sorgente
Licenza [MIT](LICENSE) — libero uso, modifica, redistribuzione per il **codice**.

### Contenuti audio
- **Non sono mia proprietà.** Tutti i diritti sui dialoghi, personaggi, opera originale appartengono a **Filmauro**, **Medusa**, **Mediaset** e agli autori/interpreti (a seconda della pellicola).
- **Provenienza:** i clip audio di default sono stati **scaricati da YouTube**, estratti da video di terzi pubblicamente accessibili. L'autore di questo progetto non è la fonte originale e non ha rippato direttamente dai master.
- **Uso:** esclusivamente illustrativo, satirico, di omaggio (*tribute*), educativo e di commento critico alla serie. Nessuno dei clip è stato alterato sostanzialmente; sono brevi estratti (pochi secondi) dal contesto integrale dell'opera.
- **Fair use / eccezioni copyright:** il progetto si appoggia ai principi di *fair use* (USA) / *fair dealing* e alle eccezioni per critica, recensione, caricatura, parodia e pastiche previste dalla direttiva UE 2019/790 art. 17(7) e dall'art. 70 L. 633/1941 (legge italiana sul diritto d'autore).

### Nessuno scopo di lucro
- ❌ Nessuna vendita, nessuna donazione, nessun annuncio pubblicitario, nessuna promozione a pagamento
- ❌ Nessun paywall, nessun abbonamento, nessuna in-app purchase
- ❌ Nessuna telemetria, nessuna analitica, nessun tracciamento utenti
- ✅ Gratis, open source, auto-contenuto, offline

### Nessuna affiliazione
De Sica Bar **non è** un prodotto ufficiale. **Non è** affiliato, sponsorizzato, approvato o in alcun modo connesso con Filmauro, Medusa, Mediaset, gli autori o gli interpreti (Cristian De Sica) o i detentori dei diritti. Ogni riferimento è puramente di omaggio tra fan.

### Takedown policy / contatto DMCA
Se sei un detentore di diritti e vuoi la rimozione dei contenuti:

📧 **Email diretta (preferita): [ale.ciano91@gmail.com](mailto:ale.ciano91@gmail.com)**  
🐛 Oppure apri una [issue](../../issues) su GitHub  

Per richieste DMCA / takedown valide (detentore dei diritti verificabile + identificazione del contenuto da rimuovere) i file saranno rimossi **entro 24 ore dalla ricezione**, in buona fede, senza contestazione legale.

### Uso da parte dei fan
Scaricando e usando De Sica Bar riconosci che:
- Lo fai per uso strettamente personale e domestico
- Non redistribuirai i clip audio a fini commerciali
- Non userai l'app in contesti pubblici monetizzati (trasmissioni, streaming a pagamento, eventi ticketed)
- L'autore non fornisce garanzie legali sul contenuto audio

---

## 🤘 Autore

Fork by **ale.ciano91@gmail.com**

> Basato su [Boris Bar](https://github.com/andrearicciotti1/boris-bar) di [Andrea Ricciotti / PunxCode](https://punxcode.com) — grazie per il progetto originale! 🙏

---

> *"Delicatissimo."*
