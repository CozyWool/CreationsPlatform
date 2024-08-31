﻿using CreationsPlatformWebApplication.Messages;
using CreationsPlatformWebApplication.Models.Creation;
using CreationsPlatformWebApplication.Models.FilterSortPaging;
using CreationsPlatformWebApplication.Models.ViewModels;
using CreationsPlatformWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreationsPlatformWebApplication.Controllers;

[Route("[controller]")]
[Authorize]
public class CreationsPlatformController : Controller
{
    private readonly ICreationService _creationService;
    private readonly IUserService _userService;
    private readonly IGenreService _genreService;
    private readonly ICommentService _commentService;
    private List<GenreModel> Genres => _genreService.GetGenres().Result;

    public CreationsPlatformController(ICreationService creationService, IUserService userService,
        IGenreService genreService, ICommentService commentService)
    {
        _creationService = creationService;
        _userService = userService;
        _genreService = genreService;
        _commentService = commentService;
    }


    [AllowAnonymous]
    [Route("/")]
    [HttpGet("index")]
    public async Task<IActionResult> Index(IndexRequest request)
    {
        var (items, count) = await _creationService.GetPagedSortedFiltered(pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            sortOrder: request.SortOrder,
            genreId: request.GenreId,
            title: request.Title,
            authorUsername: request.AuthorUsername,
            publishedBefore: request.PublishedBefore,
            publishedAfter: request.PublishedAfter);
        ViewData["Title"] = "Писательская платформа";
        var viewModel = new IndexViewModel
        {
            PageViewModel = new PageViewModel(count, request.PageNumber, request.PageSize),
            SortViewModel = new SortViewModel(request.SortOrder),
            FilterViewModel = new FilterViewModel(Genres, request.GenreId, request.Title, request.AuthorUsername,
                request.PublishedAfter, request.PublishedBefore),
            Creations = items
        };
        return View(viewModel);
    }

    [HttpGet("top-50")]
    public async Task<IActionResult> Top50(Top50Request request)
    {
        var (items, count) =
            await _creationService.GetPagedSortedFiltered(pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                sortOrder: request.SortOrder,
                authorUsername: request.AuthorUsername,
                limit: 50);
        var viewModel = new Top50ViewModel
        {
            PageViewModel = new PageViewModel(count, request.PageNumber, request.PageSize),
            SortViewModel = new SortViewModel(request.SortOrder),
            Creations = items
        };
        ViewData["Title"] = "Топ-50 произведений";
        return View("Top50", viewModel);
    }

    [HttpGet("my-creations")]
    public async Task<IActionResult> MyCreations(MyCreationsRequest request)
    {
        var username = User.Identity.Name;
        var (items, count) = await _creationService.GetPagedSortedFiltered(pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            sortOrder: request.SortOrder,
            genreId: request.GenreId,
            authorUsername: username,
            title: request.Title,
            publishedBefore: request.PublishedBefore,
            publishedAfter: request.PublishedAfter);
        ViewData["Title"] = "Писательская платформа";
        var viewModel = new IndexViewModel
        {
            PageViewModel = new PageViewModel(count, request.PageNumber, request.PageSize),
            SortViewModel = new SortViewModel(request.SortOrder),
            FilterViewModel = new FilterViewModel(Genres, request.GenreId, request.Title, request.AuthorUsername,
                request.PublishedAfter, request.PublishedBefore),
            Creations = items
        };
        ViewData["Title"] = "Мои произведения";

        return View(viewModel);
    }

    [HttpGet("id/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var creationModel = await _creationService.GetById(id);
        if (creationModel == null)
            throw new ArgumentException();
        // return NotFound(id);

        var viewModel = new DetailedCreationViewModel
        {
            Creation = creationModel,
            Comment = new CommentModel()
        };
        ViewData["Title"] = creationModel.Title;
        return View("DetailedCreation", viewModel);
    }

    [HttpPost("add-comment")]
    public async Task<IActionResult> AddComment(CommentModel comment)
    {
        ViewData["Title"] = comment.CreationTitle;
        comment.UserId = Guid.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value);
        await _commentService.Create(comment);
        return Redirect($"id/{comment.CreationId}");
    }

    [HttpDelete("delete-comment")]
    public async Task<IActionResult> AddComment(int commentId, int creationId)
    {
        if ((await _commentService.GetById(commentId)).UserId.ToString() !=
            User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value)
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        if (await _commentService.Delete(commentId))
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
            Genres = Genres
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
        creationModel.PublicationDate = DateTime.UtcNow;
        creationModel.Genres =
            creationModel
                .Genres
                .DistinctBy(e => e.Id)
                .ToList();
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
            Genres = Genres
        };
        ViewData["Title"] = $"Изменение {creationModel.Title}";
        return View("EditCreation", creationViewModel);
    }

    [HttpPost("edit")]
    public async Task<IActionResult> Edit([FromForm(Name = "Creation")] CreationModel creationModel)
    {
        if (creationModel.Author.Id.ToString() != User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value)
            return RedirectToAction("AccessDenied", "Account");

        creationModel.Genres =
            creationModel
                .Genres
                .DistinctBy(e => e.Id)
                .ToList();
        await _creationService.Update(creationModel);
        return RedirectToAction(nameof(MyCreations));
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
            Genres = Genres
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
                Genres = Genres
            };
            return View(isEdit ? "EditCreation" : "CreateCreation", creationViewModel);
        }

        creationModel.Genres.RemoveAt(index);
        creationViewModel = new CreationViewModel
        {
            Creation = creationModel,
            Genres = Genres
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
            return RedirectToAction(nameof(MyCreations));
        }

        return BadRequest();
    }
}