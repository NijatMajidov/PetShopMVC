using Microsoft.AspNetCore.Hosting;
using PetShop.Business.Exceptions;
using PetShop.Business.Services.Abstract;
using PetShop.Core.Models;
using PetShop.Core.RepositoryAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace PetShop.Business.Services.Concretes
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamService(ITeamRepository teamRepository, IWebHostEnvironment webHostEnvironment)
        {
            _teamRepository = teamRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public void CreateTeam(Team team)
        {
            if (team == null) throw new EntityNullReferanceException("", "Team Null Referance");
            if (team.ImageFile == null) throw new Exceptions.FileNotFoundException("ImageFile", "File Null Referance");
            if (!team.ImageFile.ContentType.Contains("image/")) throw new FileContentTypeException("ImageFile", "File not Image!");
            if (team.ImageFile.Length > 2097152) throw new FileSizeException("ImageFile", "File size error!");

            string filename = Guid.NewGuid().ToString() + Path.GetExtension(team.ImageFile.FileName);
            string path = _webHostEnvironment.WebRootPath +@"\uploads\teams\" + filename;
            using(FileStream stream = new FileStream(path, FileMode.Create))
            {
                team.ImageFile.CopyTo(stream);
            }
            team.ImageUrl = filename;
            _teamRepository.Add(team);
            _teamRepository.Commit();
        }

        public void DeleteTeam(int id)
        {
            var exsitsTeam = _teamRepository.Get(x=>x.Id == id);
            if (exsitsTeam == null) throw new EntityNullReferanceException("", "Team Null Referance");
            string path = _webHostEnvironment.WebRootPath + @"\uploads\teams\" + exsitsTeam.ImageUrl;
            if (!File.Exists(path)) throw new Exceptions.FileNotFoundException("ImageFile", "File Not Found");
            File.Delete(path);

            _teamRepository.Delete(exsitsTeam);
            _teamRepository.Commit();
        }

        public List<Team> GetAllTeams(Func<Team, bool>? func = null)
        {
            return _teamRepository.GetAll(func);
        }

        public Team GetTeam(Func<Team, bool>? func = null)
        {
           return _teamRepository.Get(func);
        }

        public void UpdateTeam(int id, Team newTeam)
        {
            var oldTeam = _teamRepository.Get(x => x.Id == id);
            if (oldTeam == null) throw new EntityNullReferanceException("", "Team member not found");

            if (newTeam == null) throw new EntityNullReferanceException("", "Entity null referance exception");
            if (newTeam.ImageFile != null)
            {
                if (!newTeam.ImageFile.ContentType.Contains("image/")) throw new FileContentTypeException("ImgFile", "This file not Image");
                if (newTeam.ImageFile.Length > 2097152) throw new FileSizeException("ImgFile", "File size exception. File < 2Mb!!!");
                string oldPath = _webHostEnvironment.WebRootPath + @"\uploads\teams\" + oldTeam.ImageUrl;
                if (!File.Exists(oldPath)) throw new Exceptions.FileNotFoundException("ImageFile", "File Null Referance");
                File.Delete(oldPath);
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(newTeam.ImageFile.FileName);
                string path = _webHostEnvironment.WebRootPath + @"\uploads\teams\" + fileName;

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    newTeam.ImageFile.CopyTo(stream);
                }

                oldTeam.ImageUrl = fileName;
            }
            oldTeam.Name = newTeam.Name;
            oldTeam.Surname = newTeam.Surname;
            oldTeam.Description = newTeam.Description;
            _teamRepository.Commit();

        }
    }
}
