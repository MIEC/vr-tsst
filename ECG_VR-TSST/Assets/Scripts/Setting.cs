using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class to hold values for saving the settings the user can set
/// </summary>
public class Setting{

    public bool oneAuditor;
    public bool secondRoundAnnouncement;
    public bool firstIsSecond;
    public string[] firstRound;
    public string[] secondRound;
    public byte language;

    /// <summary>
    /// constructor with parameters
    /// </summary>
    /// <param name="oneAuditor"></param>
    /// <param name="secondRoundAnnouncement"></param>
    /// <param name="firstRound"></param>
    /// <param name="secondRound"></param>
    /// <param name="language"></param>
    /// <param name="firstIsSecond"></param>
    public Setting(bool oneAuditor, bool secondRoundAnnouncement, string[] firstRound, string[] secondRound, byte language, bool firstIsSecond)
    {
        this.oneAuditor = oneAuditor;
        this.secondRoundAnnouncement = secondRoundAnnouncement;
        this.firstRound = firstRound;
        this.secondRound = secondRound;
        this.language = language;
        this.firstIsSecond = firstIsSecond;
    }

    /// <summary>
    /// standard constructor
    /// </summary>
    public Setting()
    {

    }

}
