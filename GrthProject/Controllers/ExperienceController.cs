using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using CMP.Operation.Functions;
using CMP.Operation.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Http;

namespace CMPhProject.Controllers
{
    public class ExperienceController : ApiController
    {
        #region Private/protected fields
        protected readonly RepositoryContainer _repository = new RepositoryContainer();
        #endregion

        #region Api Methods

        // GET api/experience
        [Route("api/experience/{iExperience}")]
        public IHttpActionResult Get([FromBody]int iExperience)
        {
            IEnumerable<ExperienceView> experience = null;
            try
            {
                int ExperienceCount;
                experience = _repository.ExperienceRepository.GetByFiltering(iExperience, 0, 0, null, null, out ExperienceCount);
            }
            catch { }

            return Json(new { Records = experience });
        }

        // POST api/experience
        public IHttpActionResult Post([FromBody]ExperienceViewModel ExperienceVM)
        {
            //Update or Create
            try
            {
                if (IsExperienceExists(ExperienceVM))
                {
                    return InsertExperience(ExperienceVM);
                }
                else
                {
                    return UpdateExperience(ExperienceVM);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }
        }
        // POST api/experience/5
        public IHttpActionResult Post(int ID)
        {
            try
            {
                var ObjDel = _repository.ExperienceRepository.GetField(ID);
                if (!ObjDel.IsAllowDelete)
                    return Json(new { Result = "ERROR", Message = Errors.MessageErrorAllowDelete });
                else
                {
                    Experience ExperienceDel = _repository.ExperienceRepository.Find(ID);
                    _repository.ExperienceRepository.Delete(ExperienceDel);
                    _repository.ExperienceRepository.Save();
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }
        }
        #endregion

        #region Private Methods

        private IHttpActionResult UpdateExperience(ExperienceViewModel ExperienceVM)
        {
            try
            {
                if (!IsExperienceAvailble(ExperienceVM))
                    return Json(new { Result = "ERROR", Message = "Experience Name already exists" });
                Experience ExperienceUpdated = _repository.ExperienceRepository.WhereAndInclude
                    (a => a.ID == ExperienceVM.ID, c => c.ExperienceTechnologys) as Experience;

                //update ExperienceTechnologys list (delete/remove/update)
                var expList = new Experience() { ExperienceTechnologys = GetExperienceTechnologies(ExperienceVM.Technologies, ExperienceVM.ID) };
                _repository.ExperienceRepository.UpdateChildCollection(ExperienceUpdated, expList, t => t.ExperienceTechnologys, child => child.TechnologyID);

                //update Experience Entity
                ExperienceUpdated.IsActive = ExperienceVM.IsActive;
                ExperienceUpdated.FromDate = ExperienceVM.FromDate;
                ExperienceUpdated.ToDate = ExperienceVM.ToDate;
                ExperienceUpdated.CompanyID = ExperienceVM.CompanyID;
                _repository.ExperienceRepository.Edit(ExperienceUpdated);
                if (_repository.ExperienceRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.ExperienceRepository.GetField(ExperienceVM.ID);
                    return Json(new { Result = "OK", Record = Record });
                }
                else
                    return Json(new { Result = "ERROR", Message = Errors.MessageError });
            }
            catch
            {
                return Json(new { Result = "ERROR", Message = Errors.MessageError });
            }

        }

        private IHttpActionResult InsertExperience(ExperienceViewModel ExperienceVM)
        {
            try
            {
                if (!IsExperienceAvailble(ExperienceVM))
                    return Json(new { Result = "ERROR", Message = "Experience Name already exists" });
                Experience ExperienceADD = new Experience
                {
                    EmployeeID = ExperienceVM.EmployeeID,
                    CompanyID = ExperienceVM.CompanyID,
                    FromDate = ExperienceVM.FromDate,
                    ToDate = ExperienceVM.ToDate,
                    IsActive = ExperienceVM.IsActive,
                    ExperienceTechnologys = GetExperienceTechnologies(ExperienceVM.Technologies, ExperienceVM.ID)
                };
                _repository.ExperienceRepository.Add(ExperienceADD);
                if (_repository.ExperienceRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.ExperienceRepository.GetField(ExperienceADD.ID);
                    return Json(new { Result = "OK", Record = ExperienceADD });
                }
                else
                    return Json(new { Result = "ERROR", Message = Errors.MessageError });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }
        }
        #endregion

        #region Validation
        public bool IsExperienceAvailble(ExperienceViewModel ExperienceVM)
        {
            try
            {
                bool IsOrder;

                IsOrder = !_repository.ExperienceRepository.Any(cc => cc.CompanyID == ExperienceVM.CompanyID && cc.EmployeeID == ExperienceVM.EmployeeID && cc.FromDate == ExperienceVM.FromDate && cc.ID != ExperienceVM.ID);
                return IsOrder;
            }
            catch (Exception)
            {
                return true;
            }
        }
        public bool IsExperienceExists(ExperienceViewModel Experience)
        {
            try
            {
                bool IsOrder = _repository.ExperienceRepository.Any(cc => cc.ID == Experience.ID);
                return IsOrder;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Experience Technology events
        /// <summary>
        /// gets list id's technologies and return new list of that id's
        /// </summary>
        /// <param name="TechnologiyIDs"></param>
        /// <param name="ExperianeID"></param>
        /// <returns></returns>
        private List<ExperienceTechnology> GetExperienceTechnologies(List<int> TechnologiyIDs, int ExperianeID)
        {
            List<ExperienceTechnology> exp = new List<ExperienceTechnology>();
            TechnologiyIDs.ForEach(techID =>
            {
                exp.Add(new ExperienceTechnology()
                {
                    ExperienceID = ExperianeID,
                    TechnologyID = techID
                });
            });
            return exp;
        }
        #endregion
    }
}