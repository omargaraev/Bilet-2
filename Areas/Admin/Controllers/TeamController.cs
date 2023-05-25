using Bilet_2.Areas.Admin.ViewModels;
using Bilet_2.DAL;
using Bilet_2.Models;
using Bilet_2.Utilities.Constants;
using Bilet_2.Utilities.ErrorMessages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Bilet_2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {

        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _context.Teams.OrderByDescending(t => t.Id).ToListAsync();
            return View(teams);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamVM teamVM)
        {
            if (!ModelState.IsValid)
            {
                return View(teamVM);
            }

            if (!teamVM.Photo.CheckImageType("/image"))
            {
                ModelState.AddModelError("Photo",Messages.MustBeImageType);
            }
            if (!teamVM.Photo.CheckImageSize(200))
            {
                ModelState.AddModelError("Photo", Messages.MustBeLessThan200kb);
            }

            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath,"assets","img","team");
            string fileName = Guid.NewGuid().ToString() + teamVM.Photo.FileName;

            using (FileStream fileStream = new FileStream(Path.Combine(rootPath, fileName), FileMode.Create))
            {
               await  teamVM.Photo.CopyToAsync(fileStream);
            }

            Team team = new Team {
                Name= teamVM.Name,
                Possition=teamVM.Possition,
                ImagePath =fileName
            };

            await _context.Teams.AddAsync(team);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }



        public async Task<IActionResult> Update(int id)
        {
            Team team = await _context.Teams.FindAsync(id);
          if (team == null) { return NotFound(); }

            UpdateTeamVM updateTeam = new UpdateTeamVM 
            {
                Name = team.Name,
                Possition= team.Possition,
                Id= id
                
            };
          return View(updateTeam);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateTeamVM teamVM)
        {
            if (!ModelState.IsValid)
            {
                return View(teamVM);
            }

            if (!teamVM.Photo.ContentType.Contains("/image"))
            {
                ModelState.AddModelError("Photo", Messages.MustBeImageType);
            }
            if (teamVM.Photo.Length / 1024 > 200)
            {
                ModelState.AddModelError("Photo", Messages.MustBeLessThan200kb);
            }
            Team team = await _context.Teams.FindAsync(teamVM.Id);


            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team");
            string oldFileName = Path.Combine(rootPath,team.ImagePath);

            if (System.IO.File.Exists(oldFileName))
            {
                System.IO.File.Delete(oldFileName);
            }

            
            string newfileName = Guid.NewGuid().ToString() + teamVM.Photo.FileName;

            using (FileStream fileStream = new FileStream(Path.Combine(rootPath, newfileName), FileMode.Create))
            {
                await teamVM.Photo.CopyToAsync(fileStream);
            }

            team.Name = teamVM.Name;
            team.Possition = teamVM.Possition;
            team.ImagePath = newfileName;

           await  _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }




        public async Task<IActionResult> Delete(int id)
        {
            Team team = await _context.Teams.FindAsync(id);
            if (team == null) { return NotFound(); }


            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath,"assets","img","team",team.ImagePath);

            if (System.IO.File.Exists(rootPath))
            {
                System.IO.File.Delete(rootPath);
            }

            _context.Teams.Remove(team);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));


        }
    }
}
