using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharpQuiz.Database;
using SharpQuiz.Domain.Entity;
using SharpQuiz.Models;

namespace SharpQuiz.Controllers;
// [ApiController]
// [Route("/v{version:apiVersion}/[Controller]/[Action]")]
// [Produces("application/json")]
// [ApiVersion("1")]
public class ClauseController : Controller
{
    private readonly ILogger<ClauseController> _logger;
    
    private readonly DatabaseContext _context;
    // private readonly NLog.ILogger _logger = LogManager.GetLogger(nameof(ReportController));
    // private readonly IJobGenerateReportService _generateReportService;
    // private readonly IAuthorizationHelper _authorizationHelper;
    // private readonly IFileProvider _fileProvider;
    // private readonly FileStorageSettings _fileStorageSettings;

    public ClauseController(ILogger<ClauseController> logger,
        DatabaseContext context
    // IJobGenerateReportService generateReportService,
    //     IAuthorizationHelper authorizationHelper,
    // IFileProvider fileProvider,
    //     FileStorageSettings fileStorageSettings)
    )
    {
        _logger = logger;
        _context = context;
        // _generateReportService = generateReportService;
        // _authorizationHelper = authorizationHelper;
        // _fileProvider = fileProvider;
        // _fileStorageSettings = fileStorageSettings;
    }
    
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var сlause = await _context.Clauses.OrderBy(p => p.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalClause = _context.Clauses.Count();

        var model = new PaginatedList<Clause>(сlause, totalClause, page, pageSize);

        return View(model);
    }
    
    // Action method to display form for adding a new product
   public IActionResult Create()
   {
       TempData["AddSuccess"] = null;
       TempData["AddError"] = null;
       TempData["UpdateSuccess"] = null;
       TempData["UpdateError"] = null;
       return View();
   }

   [HttpPost]
   public async Task<IActionResult> Create(Clause clause)
   {
       TempData["AddSuccess"] = null;
       TempData["AddError"] = null;

       if (ModelState.IsValid)
       {
           try
           {
               // Set CreateDateTime and ModifieDateTime
               // product.CreateDateTime = DateTime.Now;
               // product.ModifieDateTime = DateTime.Now;

               // Add the product to the database
               _context.Clauses.Add(clause);
               await _context.SaveChangesAsync();

               TempData["AddSuccess"] = $"ClauseID {clause.Id} has been added successfully.";

               // Redirect to the product list page after successful creation
               return RedirectToAction("Index");
           }
           catch (Exception ex)
           {
               // Log or handle the exception appropriately
               TempData["AddError"] = $"Exception while adding the new book : {ex.Message}";
           }
       }

       // If the model state is not valid, return the view with the model data and errors
       return View(clause);
   }
    
    // GET
    // [HttpPost]
    // [ProducesResponseType(typeof(CreateReportResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status401Unauthorized)]
    // [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
    // [HttpGet]
    // public async Task<IActionResult> Index()
    // {
    //     var books = await _context.Books.ToListAsync();
			 //
    //     return View(books);
    //     
    //     // try
    //     // {
    //     //     if (!(await _authorizationHelper.CheckAuthorization<BaseAuthorizationHandler.CanCreateReportRequirement>()).Succeeded) return Forbid();
    //     //
    //     //     var response = await _generateReportService.CreateGenerateReportJobAsync(request);
    //     //     return Ok(response);
    //     // }
    //     // catch(EntityNotFoundException ex)
    //     // {
    //     //     return NotFound($"Issue has failed. Reason: {ex.Message}");
    //     // }
    //     // catch (ValidationException ex)
    //     // {
    //     //     _logger.Error($"Issue has failed. Reason: {ex.Message}");
    //     //     return BadRequest(ex.Message);
    //     // }
    //     // catch (Exception ex)
    //     // {
    //     //     _logger.Error($"Issue has failed. Reason: {ex.Message}");
    //     //     return BadRequest("Не удалось создать задачу на формирование отчёта");
    //     // }
    // }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
