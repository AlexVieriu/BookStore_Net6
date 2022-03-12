namespace BookStore.Api.Static;
public static class ErrorMsg
{
    public static string Error500(Exception ex)
    {
        return $"{ex.InnerException} - {ex.Message}";
    }

}
