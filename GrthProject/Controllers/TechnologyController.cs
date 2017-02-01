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
    public class TechnologyController : ApiController
    {
        #region Private/protected fields
        protected readonly RepositoryContainer _repository = new RepositoryContainer();
        #endregion

        #region Api Methods

        // GET api/technology
        [Route("api/technology/{sort}")]
        public IHttpActionResult Get([FromBody]string sort)
        {
            IEnumerable<TechnologyView>Technology = null;
            try
            {
                int technologyCount;
               Technology = _repository.TechnologyRepository.GetByFiltering(0, 0, null, null, out technologyCount);
            }
            catch { }

            return Json(new { Records =Technology });
        }
        // GET api/technology/options
        [Route("api/technology")]
        public IHttpActionResult Get()
        {
            List<Options> Options = null;
            try
            {
                Options = _repository.TechnologyRepository.GetOptionsNoAsync(opt => new Options() { DisplayText = opt.Name, Value = opt.ID }, opt => opt.IsActive == true);
            }
            catch { }
            return Json(new { Options = Options });
        }
        // POST api/technology
        public IHttpActionResult Post([FromBody]TechnologyViewModel TechnologyVM)
        {
            //Update or Create
            try
            {
                if (IstechnologyExists(TechnologyVM))
                {
                    return Inserttechnology(TechnologyVM);
                }
                else
                {
                    return Updatetechnology(TechnologyVM);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }
        }
        // POST api/technology/5
        public IHttpActionResult Post(int ID)
        {
            try
            {
                var ObjDel = _repository.TechnologyRepository.GetField(ID);
                if (!ObjDel.IsAllowDelete)
                    return Json(new { Result = "ERROR", Message = Errors.MessageErrorAllowDelete });
                else
                {
                   Technology technologyDel = _repository.TechnologyRepository.Find(ID);
                    _repository.TechnologyRepository.Delete(technologyDel);
                    _repository.TechnologyRepository.Save();
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

        private IHttpActionResult Updatetechnology(TechnologyViewModel TechnologyVM)
        {
            try
            {
                if (!IsNametechnologyAvailble(TechnologyVM.Name, TechnologyVM))
                    return Json(new { Result = "ERROR", Message = "technology Name already exists" });
                Technology TechnologyUpdated = _repository.TechnologyRepository.Find(TechnologyVM.ID);
                TechnologyUpdated.Name = TechnologyVM.Name;
                TechnologyUpdated.IsActive = TechnologyVM.IsActive;
                _repository.TechnologyRepository.Edit(TechnologyUpdated);
                if (_repository.TechnologyRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.TechnologyRepository.GetField(TechnologyVM.ID);
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

        private IHttpActionResult Inserttechnology(TechnologyViewModel TechnologyVM)
        {
            try
            {
                if (!IsNametechnologyAvailble(TechnologyVM.Name, TechnologyVM))
                    return Json(new { Result = "ERROR", Message = "technology Name already exists" });
                Technology TechnologyADD = new Technology { Name = TechnologyVM.Name, IsActive = TechnologyVM.IsActive };
                _repository.TechnologyRepository.Add(TechnologyADD);
                if (_repository.TechnologyRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.TechnologyRepository.GetField(TechnologyADD.ID);
                    return Json(new { Result = "OK", Record = TechnologyADD });
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
        public bool IsNametechnologyAvailble(string Name, TechnologyViewModel technology)
        {
            try
            {
                bool IsOrder = !_repository.TechnologyRepository.Any(cc => cc.Name.Trim().ToLower() == Name.Trim().ToLower() && cc.ID != technology.ID);
                return IsOrder;
            }
            catch (Exception)
            {
                return true;
            }
        }
        public bool IstechnologyExists(TechnologyViewModel technology)
        {
            try
            {
                bool IsOrder = _repository.TechnologyRepository.Any(cc => cc.ID == technology.ID);
                return IsOrder;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}