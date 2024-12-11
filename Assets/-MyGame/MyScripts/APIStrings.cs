
public static class APIStrings
{
    static string prefixURL => "https://kryzer.fitnessinfo.uk/wp-json/";

    #region APIs

    public static string getDelayTimeBetweenRoundsAPIURL => prefixURL + "game/v1/delay";
    public static string getPlayerDetailAPIURL => prefixURL + "game/v1/add-player";
    public static string getCrashPointAPIURL => prefixURL + "randomnumber/v1/generate";
    public static string sendBetToServerAPIURL => prefixURL + "betting/v1/place-bet";
    public static string cashoutOnBtnAPIURL => prefixURL + "betting/v1/manual-crash";
    public static string getLeaderBoardAPIURL => prefixURL + "ranked-players/v1/all";

    #endregion
}

