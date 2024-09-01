using Microsoft.AspNetCore.Mvc;

namespace CreationsPlatformWebApplication.Messages;

public class DetailedCreationRequest
{
    [BindProperty(Name = "id")] public int Id { get; set; }
    [BindProperty(Name = "page")] public int PageNumber { get; set; } = 1;
    [BindProperty(Name = "pageSize")] public int PageSize { get; set; } = 325;
}