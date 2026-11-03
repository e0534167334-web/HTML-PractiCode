namespace HtmlTools;

/// <summary>
/// פונקציות הרחבה לחיפוש אלמנטים בעץ לפי Selector.
/// </summary>
public static class HtmlElementExtensions
{
    /// <summary>
    /// מחזיר את כל האלמנטים שעונים לסלקטור.
    /// HashSet מונע כפילויות (אלמנט שעונה לתנאי דרך כמה אבות).
    /// </summary>
    public static HashSet<HtmlElement> FindElements(this HtmlElement element, Selector selector)
    {
        var result = new HashSet<HtmlElement>();
        FindRecursive(element, selector, result);
        return result;
    }

    /// <summary>גרסה נוחה שמקבלת מחרוזת שאילתה ישירות</summary>
    public static HashSet<HtmlElement> Query(this HtmlElement element, string query)
        => element.FindElements(Selector.Parse(query));

    private static void FindRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
    {
        // כל הצאצאים של האלמנט (Skip(1) - בלי האלמנט עצמו, כמו ב-CSS)
        var matches = element.Descendants()
                             .Skip(1)
                             .Where(selector.Matches);

        // תנאי עצירה - הסלקטור האחרון
        if (selector.Child == null)
        {
            foreach (var match in matches)
                result.Add(match);
            return;
        }

        // ממשיכים לרמה הבאה של הסלקטור, מתוך כל אלמנט שנמצא
        foreach (var match in matches)
            FindRecursive(match, selector.Child, result);
    }
}
