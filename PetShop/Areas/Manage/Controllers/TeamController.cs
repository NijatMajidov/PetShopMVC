using PetShop.Business.Exceptions;
using PetShop.Business.Services.Abstract;
using PetShop.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PetShop.Areas.Manage.Controllers
{
    [Area("Manage")]
 [Authorize(Roles = "Admin")]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        public IActionResult Index()
        {
            return View(_teamService.GetAllTeams());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Team team)
        {
            if (!ModelState.IsValid) return View();
            try
            {
                _teamService.CreateTeam(team);
            }catch(EntityNullReferanceException ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View();
            }catch(Business.Exceptions.FileNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName,ex.Message);
                return View();
            }
            catch (FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var exsistTeam = _teamService.GetTeam(x=>x.Id == id);
            if (exsistTeam == null) return View("Error");
            return View(exsistTeam);
        }
        [HttpPost]
        public IActionResult DeleteTeam(int id)
        {
            var exsistTeam = _teamService.GetTeam(x => x.Id == id);
            if (exsistTeam == null) return View("Error");

            try
            {
                _teamService.DeleteTeam(id);
            }
            catch (EntityNullReferanceException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName,ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id) 
        {
            var exsistTeam = _teamService.GetTeam(x => x.Id == id);
            if (exsistTeam == null) return View("Error");
            return View(exsistTeam);
        }
        [HttpPost]
        public IActionResult Update(Team newTeam)
        {
            if(!ModelState.IsValid) return View();
            try
            {
                _teamService.UpdateTeam(newTeam.Id,newTeam);
            }
            catch (EntityNullReferanceException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
