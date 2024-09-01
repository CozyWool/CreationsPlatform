using CreationsPlatformWebApplication.Messages;
using CreationsPlatformWebApplication.Models.Creation;
using CreationsPlatformWebApplication.Models.ViewModels;
using CreationsPlatformWebApplication.Models.ViewModels.FilterSortPaging;
using CreationsPlatformWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreationsPlatformWebApplication.Controllers;

[Route("creations-platform")]
[Authorize]
public class CreationsPlatformController(
    ICreationService creationService,
    IUserService userService,
    IGenreService genreService,
    ICommentService commentService)
    : Controller
{
    private List<GenreModel> Genres => genreService.GetGenres().Result;


    [AllowAnonymous]
    [Route("/")]
    [HttpGet("index")]
    public async Task<IActionResult> Index(IndexRequest request)
    {
        var (items, count) = await creationService.GetPagedSortedFiltered(pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            sortOrder: request.SortOrder,
            genreId: request.GenreId,
            title: request.Title,
            authorUsername: request.AuthorUsername,
            publishedBefore: request.PublishedBefore,
            publishedAfter: request.PublishedAfter,
            ratingBefore: request.RatingBefore,
            ratingAfter: request.RatingAfter);
        ViewData["Title"] = "Писательская платформа";
        var viewModel = new IndexViewModel
        {
            PageViewModel = new PageViewModel(count, request.PageNumber, request.PageSize),
            SortViewModel = new SortViewModel(request.SortOrder),
            FilterViewModel = new FilterViewModel(Genres, request.GenreId, request.Title, request.AuthorUsername,
                request.PublishedAfter, request.PublishedBefore, request.RatingBefore, request.RatingAfter),
            Creations = items
        };
        return View(viewModel);
    }

    [HttpGet("top-50")]
    public async Task<IActionResult> Top50(Top50Request request)
    {
        var (items, count) =
            await creationService.GetPagedSortedFiltered(pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                sortOrder: request.SortOrder,
                genreId: request.GenreId,
                authorUsername: request.AuthorUsername,
                limit: 50);
        var viewModel = new Top50ViewModel
        {
            PageViewModel = new PageViewModel(count, request.PageNumber, request.PageSize),
            SortViewModel = new SortViewModel(request.SortOrder),
            FilterViewModel = new FilterViewModel(Genres, request.GenreId, "", "", null, null, null, null),
            Creations = items
        };
        ViewData["Title"] = "Топ-50 произведений";
        return View("Top50", viewModel);
    }

    [HttpGet("my-creations")]
    public async Task<IActionResult> MyCreations(MyCreationsRequest request)
    {
        var username = User.Identity.Name;
        var (items, count) = await creationService.GetPagedSortedFiltered(pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            sortOrder: request.SortOrder,
            genreId: request.GenreId,
            authorUsername: username,
            isAuthorUsernameStrict: true,
            title: request.Title,
            publishedBefore: request.PublishedBefore,
            publishedAfter: request.PublishedAfter,
            ratingBefore: request.RatingBefore,
            ratingAfter: request.RatingAfter);
        ViewData["Title"] = "Писательская платформа";
        var viewModel = new IndexViewModel
        {
            PageViewModel = new PageViewModel(count, request.PageNumber, request.PageSize),
            SortViewModel = new SortViewModel(request.SortOrder),
            FilterViewModel = new FilterViewModel(Genres, request.GenreId, request.Title, username,
                request.PublishedAfter, request.PublishedBefore, request.RatingBefore, request.RatingAfter),
            Creations = items
        };
        ViewData["Title"] = "Мои произведения";

        return View(viewModel);
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details(DetailedCreationRequest request)
    {
        var (creationModel, count) =
            await creationService.GetByIdPaged(request.Id, request.PageSize, request.PageNumber);
        if (creationModel == null)
            return NotFound();
        creationModel.Comments = creationModel.Comments.OrderByDescending(model => model.PublicationDate).ToList();
        var viewModel = new DetailedCreationViewModel
        {
            Creation = creationModel,
            PageViewModel = new PageViewModel(count, request.PageNumber, request.PageSize),
            Comment = new CommentModel()
        };
        ViewData["Title"] = creationModel.Title;
        return View("Details", viewModel);
    }

    [HttpPost("add-comment")]
    public async Task<IActionResult> AddComment(CommentModel comment)
    {
        ViewData["Title"] = comment.CreationTitle;
        if (string.IsNullOrEmpty(comment.Content))
        {
            return await Details(new DetailedCreationRequest {Id = comment.CreationId});
        }

        comment.UserId = Guid.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value);
        await commentService.Create(comment);
        return Redirect($"{nameof(Details).ToLower()}?id={comment.CreationId}");
    }

    [HttpDelete("delete-comment")]
    public async Task<IActionResult> DeleteComment(int commentId, int creationId)
    {
        if ((await commentService.GetById(commentId)).UserId.ToString() !=
            User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value)
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        if (await commentService.Delete(commentId))
        {
            return Redirect($"id/{creationId}");
        }

        return BadRequest();
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var creationViewModel = new CreationViewModel
        {
            Creation = new CreationModel(),
            Genres = Genres,
        };
        ViewData["Title"] = "Публикация произведения";
        return View("CreateCreation", creationViewModel);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm(Name = "Creation")] CreationModel creationModel)
    {
        creationModel.Author =
            await userService
                .GetAuthorById
                    (Guid.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value));
        creationModel.PublicationDate = DateTime.UtcNow;
        creationModel.Genres =
            creationModel
                .Genres
                .DistinctBy(e => e.Id)
                .ToList();
        await creationService.Create(creationModel);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit([FromRoute] int id)
    {
        var creationModel = await creationService.GetById(id);
        if (creationModel == null) return BadRequest();
        if (creationModel.Author.Id.ToString() != User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value)
            return RedirectToAction("AccessDenied", "Account");

        var creationViewModel = new CreationViewModel
        {
            Creation = creationModel,
            Genres = Genres,
        };
        ViewData["Title"] = $"Изменение {creationModel.Title}";
        return View("EditCreation", creationViewModel);
    }

    [HttpPost("edit/{id:int}")]
    public async Task<IActionResult> Edit([FromForm(Name = "Creation")] CreationModel creationModel)
    {
        if (creationModel.Author.Id.ToString() != User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value)
            return RedirectToAction("AccessDenied", "Account");

        creationModel.Genres =
            creationModel
                .Genres
                .DistinctBy(e => e.Id)
                .ToList();
        await creationService.Update(creationModel);
        return RedirectToAction(nameof(MyCreations));
    }

    [HttpPost("add-genre")]
    public IActionResult AddGenre([FromForm(Name = "Creation")] CreationModel creationModel, [FromQuery] bool isEdit)
    {
        var creationViewModel = new CreationViewModel
        {
            Genres = Genres,
            Creation = creationModel
        };

        creationModel.Genres.Add(new GenreModel
        {
            Id = -1,
            Name = "Выберите жанр"
        });
        ViewData["Title"] = "Публикация произведения";
        return View(isEdit ? "EditCreation" : "CreateCreation", creationViewModel);
    }

    [HttpPost("remove-genre/{index:int}")]
    public IActionResult RemoveGenre(int index,
        [FromForm(Name = "Creation")] CreationModel creationModel,
        [FromQuery] bool isEdit)
    {
        ViewData["Title"] = "Публикация произведения";
        CreationViewModel? creationViewModel;
        if (index < 0 || index >= creationModel.Genres.Count)
        {
            creationViewModel = new CreationViewModel
            {
                Creation = creationModel,
                Genres = Genres
            };
            return View(isEdit ? "EditCreation" : "CreateCreation", creationViewModel);
        }


        creationModel.Genres.RemoveAt(index);
        creationViewModel = new CreationViewModel
        {
            Genres = Genres,
            Creation = creationModel
        };
        return View(isEdit ? "EditCreation" : "CreateCreation", creationViewModel);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if ((await creationService.GetById(id)).Author.Id.ToString() !=
            User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value)
            return RedirectToAction("AccessDenied", "Account");
        if (await creationService.Delete(id))
        {
            return RedirectToAction(nameof(MyCreations));
        }

        return BadRequest();
    }
}