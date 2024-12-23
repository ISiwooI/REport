using System.Text.RegularExpressions;

static class RegularExpressions
{
    public static string anyTextPattern = @"([\s\S]*?)";

}
public struct RPSurroundText
{
    public string Startstr;
    public string Endstr;
    public string regPattern;
    public RPSurroundText(string startstr, string endstr)
    {
        this.Startstr = startstr;
        this.Endstr = endstr;
        this.regPattern = Regex.Escape(Startstr) + RegularExpressions.anyTextPattern + Regex.Escape(Endstr);
    }
}
