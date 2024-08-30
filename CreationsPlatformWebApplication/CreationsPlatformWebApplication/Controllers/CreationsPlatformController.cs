using CreationsPlatformWebApplication.Models;
using CreationsPlatformWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreationsPlatformWebApplication.Controllers;

[Route("{controller}")]
[Authorize]
public class CreationsPlatformController : Controller
{
    private readonly ICreationService _creationService;
    private readonly IUserService _userService;
    private readonly IGenreService _genreService;
    private List<GenreModel> _genres => _genreService.GetGenres().Result;

    public CreationsPlatformController(ICreationService creationService, IUserService userService,
        IGenreService genreService)
    {
        _creationService = creationService;
        _userService = userService;
        _genreService = genreService;
    }

    [AllowAnonymous]
    [Route("/")]
    [HttpGet("index")]
    public async Task<IActionResult> Index()
    {
        var viewModel = await _creationService.GetAll();
        ViewData["Title"] = "Писательская платформа";

        return View(viewModel);
    }
    [HttpGet("my-creations")]
    public async Task<IActionResult> MyCreations()
    {
        var userId = Guid.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value);
        var viewModel = await _creationService.GetUsersCreations(userId);
        ViewData["Title"] = "Мои произведения";

        return View(viewModel);
    }

    [HttpGet("id/{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var creationModel = await _creationService.GetById(id);
        if (creationModel == null)
            throw new ArgumentException();
        // return NotFound(id);

        ViewData["Title"] = creationModel.Title;

        return View("DetailedCreation", creationModel);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var creationViewModel = new CreationViewModel
        {
            Creation = new CreationModel(),
            Genres = _genres
        };
        ViewData["Title"] = "Публикация произведения";
        return View("CreateCreation", creationViewModel);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm(Name = "Creation")] CreationModel creationModel)
    {
        creationModel.Author =
            await _userService
                .GetAuthorById
                    (Guid.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value));
        creationModel.PublicationDate = DateTime.Now;
        creationModel.Genres = await _genreService
            .FillNameField(
                creationModel
                    .Genres
                    .DistinctBy(e => e.Id)
                    .ToList());
        await _creationService.Create(creationModel);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit([FromRoute] int id)
    {
        var creationModel = await _creationService.GetById(id);
        if (creationModel == null) return BadRequest();
        if (creationModel.Author.Id.ToString() != User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value)
            return RedirectToAction("AccessDenied", "Account");

        var creationViewModel = new CreationViewModel
        {
            Creation = creationModel,
            Genres = _genres
        };
        ViewData["Title"] = $"Изменение {creationModel.Title}";
        return View("EditCreation", creationViewModel);
    }

    [HttpPost("edit")]
    public async Task<IActionResult> Edit([FromForm(Name = "Creation")] CreationModel creationModel)
    {
        if (creationModel.Author.Id.ToString() != User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value)
            return RedirectToAction("AccessDenied", "Account");

        creationModel.Genres = await _genreService
            .FillNameField(
                creationModel
                    .Genres
                    .DistinctBy(e => e.Id)
                    .ToList());
        await _creationService.Update(creationModel);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("add-genre")]
    public IActionResult AddGenre([FromForm(Name = "Creation")] CreationModel creationModel, [FromQuery] bool isEdit)
    {
        creationModel.Genres.Add(new GenreModel
        {
            Id = -1,
            Name = "Выберите жанр"
        });
        var creationViewModel = new CreationViewModel
        {
            Creation = creationModel,
            Genres = _genres
        };

        ViewData["Title"] = "Публикация произведения";
        return View(isEdit ? "EditCreation" : "CreateCreation", creationViewModel);
    }

    [HttpPost("remove-genre/{index:int}")]
    public IActionResult RemoveGenre(int index, [FromForm(Name = "Creation")] CreationModel creationModel,
        [FromQuery] bool isEdit)
    {
        ViewData["Title"] = "Публикация произведения";
        CreationViewModel? creationViewModel;
        if (index < 0 || index >= creationModel.Genres.Count)
        {
            creationViewModel = new CreationViewModel
            {
                Creation = creationModel,
                Genres = _genres
            };
            return View(isEdit ? "EditCreation" : "CreateCreation", creationViewModel);
        }

        creationModel.Genres.RemoveAt(index);
        creationViewModel = new CreationViewModel
        {
            Creation = creationModel,
            Genres = _genres
        };

        return View(isEdit ? "EditCreation" : "CreateCreation", creationViewModel);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if ((await _creationService.GetById(id)).Author.Id.ToString() !=
            User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value)
            return RedirectToAction("AccessDenied", "Account");
        if (await _creationService.Delete(id))
        {
            return Ok();
        }

        return BadRequest();
    }
}