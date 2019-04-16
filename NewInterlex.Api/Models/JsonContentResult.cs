namespace NewInterlex.Api.Models
{
    using Microsoft.AspNetCore.Mvc;

    public class JsonContentResult : ContentResult
    {
        public JsonContentResult()
        {
            this.ContentType = "application/json";
        }
    }
}