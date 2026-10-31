// Codice originale di MediEvilRecomp (BlackLabelHQ), adattato alla versione PAL italiana.
// MediEvilRecomp non ha una licenza esplicita: i diritti su questo codice restano ai suoi
// autori. Questo file NON e' rilasciato sotto GPL-3.0: la GPL del fork copre solo i file
// scritti da zero per il port (vedi la sezione Licenza del README).
// Indirizzi e costanti PAL: https://github.com/Leproide

using RecompOne.Runtime.Events;
using RecompOne.Runtime.Host.Window;

namespace Recompiled;

public static class QualityOfLifeMenu
{
    public static void Register()
    {
        QualityOfLife.Load();
        Event.AddListener<RuntimeReadyEvent>(_ => QualityOfLife.Load());
        
        foreach (var option in QualityOfLifeOptions.All)
            option.Changed += _ => QualityOfLife.Load();

        PanelManager.Register(new QualityOfLifePanel());

        MenuRegistry.Menu("menu.misc").After("menu.mods").Panel<QualityOfLifePanel>("panel.qol");
    }
}
