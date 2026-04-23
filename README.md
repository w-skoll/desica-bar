# 🐟 De Sica Bar

> ⚠️ **FAN PROJECT NON COMMERCIALE — NON-COMMERCIAL FAN PROJECT**  
> Questo è un progetto amatoriale gratuito, open source, senza scopo di lucro né valore commerciale, creato da un fan per uso personale e di altri fan. Non è affiliato, sponsorizzato, approvato o in alcun modo connesso con Filmauro, Medusa, Mediaset, o con gli autori, registi, interpreti o detentori dei diritti dei film. Tutti i marchi, titoli, personaggi, dialoghi e opere derivate sono proprietà dei rispettivi titolari.  
> I clip audio usati a scopo dimostrativo sono stati scaricati da YouTube (contenuti pubblicamente accessibili caricati da terzi) e usati qui esclusivamente per scopo illustrativo, satirico e di omaggio. **Nessun ricavo, donazione, pubblicità o monetizzazione è associato a questo progetto.**  
> Se sei il titolare dei diritti e desideri la rimozione dei contenuti, scrivi a **ale.ciano91@gmail.com** o apri una [issue](../../issues): i file verranno rimossi tempestivamente, entro 24 ore, senza discussione.

---

Menu bar app macOS ispirata alle frasi di Cristian De Sica. Clip audio iconici a portata di shortcut globale, dalla menu bar del Mac.

Ultra-leggera (~4 MB RAM idle), zero dipendenze, Swift nativo + Cocoa + AVFoundation.

![screenshot](docs/screenshot.png)

---

## ✨ Features

- 🎣 Icona pesce rosso nella menu bar (invisibile nel Dock)
- 🎧 13 clip audio con shortcut globali `⌥⌘1` … `⌥⌘[`
- ➕ Carica i tuoi suoni personalizzati via file picker
- ⏱ Durata max 30s per clip, toggle start/stop premendo di nuovo lo shortcut
- 🚀 Avvia al login (opzionale, togglable dal menu)
- 🪶 Binario 100 KB, nessun Electron, nessun Python

---

## 📦 Installazione

### Opzione A — DMG (consigliata)

1. Scarica `DeSicaBar-1.0.dmg` dall'ultima [Release](../../releases/latest)
2. Aprilo e trascina `DeSicaBar.app` nella cartella `Applications`
3. **Primo avvio — rimuovere la quarantena:** l'app non è firmata Apple Developer ID, quindi macOS la marca come "danneggiata". Apri Terminale e lancia una volta:
   ```bash
   xattr -cr /Applications/DeSicaBar.app
   ```
   Poi apri l'app normalmente col doppio click.  
   (Alternativa: tasto destro → **Apri** → **Apri** — funziona solo su versioni vecchie di macOS, su Sequoia / Tahoe serve il comando sopra.)
4. Clicca il pesce nella menu bar → scegli un clip.

### Opzione B — Build dai sorgenti

```bash
git clone https://github.com/ale-ciano91/desica-bar.git
cd desica-bar
# Metti i tuoi file audio (mp3/wav/m4a) in assets/clips/
./build.sh
open DeSicaBar.app
```

Richiede macOS 13+ e Command Line Tools (`xcode-select --install`).

---

## 🎵 Aggiungere suoni personalizzati

Dal menu dell'app:

- **"Aggiungi suono personalizzato…"** → file picker → il suono viene copiato in  
  `~/Library/Application Support/DeSicaBar/custom/` e compare nel menu
- **"Apri cartella suoni"** → per rinominare/cancellare manualmente

Formati supportati: `mp3`, `mp4`, `m4a`, `wav`, `aiff`, `caf`.  
Nome file = label nel menu.

---

## ⌨️ Shortcut globali predefiniti

| Shortcut | Clip |
|----------|------|
| `⌥⌘1`    | A Dragon Ball |
| `⌥⌘2`    | Aggiudicato |
| `⌥⌘3`    | Ansia terribile |
| `⌥⌘4`    | Category |
| `⌥⌘5`    | Delicatissimi |
| `⌥⌘6`    | Fucilata all'incrocio |
| `⌥⌘7`    | Ma chi so' Mission Impossible |
| `⌥⌘8`    | Ma vattene a fa' |
| `⌥⌘9`    | Na bella figura de merda |
| `⌥⌘0`    | Scherzo innocente, una burla |
| `⌥⌘-`    | Sono un troione |
| `⌥⌘=`    | Stendere un velo |
| `⌥⌘[`    | Sto a scherzà, sto a scherzà |

Funzionano ovunque, anche senza aprire il menu. Ripremere lo stesso shortcut ferma la riproduzione.

---

## 🛠 Stack tecnico

- **Swift** (single-file `desica_bar.swift`, ~200 righe)
- **Cocoa** — NSStatusItem, NSMenu
- **AVFoundation** — AVAudioPlayer
- **ServiceManagement** — SMAppService per login item (macOS 13+)
- **Carbon.HIToolbox** — RegisterEventHotKey per shortcut globali (zero permessi accessibility)

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

L'autore si impegna a rispettare ogni richiesta legittima di takedown.

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
