using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using CMP.Operation.Functions;
using CMP.Operation.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;

namespace CMPhProject.Controllers
{
    public class CitiesController : ApiController
    {
        #region Private/protected fields
        protected readonly RepositoryContainer _repository = new RepositoryContainer();
        #endregion

        #region Api Methods

        // GET api/cities
        [Route("api/cities/{sort}")]
        public IHttpActionResult Get([FromBody]string sort)
        {
            IEnumerable<CityView> citiesList = null;
            try
            {
                int CityCount;
                 citiesList = _repository.CityRepository.GetByFiltering(0, 0, null, null, out CityCount);
            }
            catch { }

            return Json(new { Records = citiesList });
        }
        // GET api/cities/iStateID
        [Route("api/cities/{iStateID}")]
        public IHttpActionResult Get([FromUri]int iStateID)
        {
            List<Options> Options = null;
            try
            {
                Options =  _repository.CityRepository.GetOptionsNoAsync(opt => new Options() { DisplayText = opt.Name,
                    Value = opt.ID }, opt => opt.IsActive == true && opt.StateID == iStateID);
            }
            catch { }
            return Json(new { Options = Options });
        }
        // POST api/cities
        public IHttpActionResult Post([FromBody]CityViewModel CityVM)
        {
            //Update or Create
            try
            {
                if (IsCityExists(CityVM))
                {
                    return  InsertCity(CityVM);
                }
                else
                {
                    return  UpdateCity(CityVM);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }
        }
        // POST api/cities/5
        public IHttpActionResult Post(int ID)
        {
            try
            {
                var ObjDel = _repository.CityRepository.GetField(ID);
                if (!ObjDel.IsAllowDelete)
                    return Json(new { Result = "ERROR", Message = Errors.MessageErrorAllowDelete });
                else
                {
                    City CityDel = _repository.CityRepository.Find(ID);
                    _repository.CityRepository.Delete(CityDel);
                     _repository.CityRepository.Save((WindowsPrincipal)User);
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

        private IHttpActionResult UpdateCity(CityViewModel CityVM)
        {
            try
            {
                if (!IsNameCityAvailble(CityVM.Name, CityVM))
                    return Json(new { Result = "ERROR", Message = "City Name already exists" });
                City CityUpdated = _repository.CityRepository.Find(CityVM.ID);
                CityUpdated.Name = CityVM.Name;
                CityUpdated.IsActive = CityVM.IsActive;
                CityUpdated.StateID = CityVM.StateID;
                _repository.CityRepository.Edit(CityUpdated);
                if ( _repository.CityRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.CityRepository.GetField(CityVM.ID);
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

        private IHttpActionResult InsertCity(CityViewModel CityVM)
        {
            try
            {
                if (!IsNameCityAvailble(CityVM.Name, CityVM))
                    return Json(new { Result = "ERROR", Message = "City Name already exists" });
                City CityADD = new City
                {
                    Name = CityVM.Name,
                    IsActive = CityVM.IsActive,
                    StateID = CityVM.StateID,
                    IsSystem = false
                };
                _repository.CityRepository.Add(CityADD);
                if ( _repository.CityRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.CityRepository.GetField(CityADD.ID);
                    return Json(new { Result = "OK", Record = CityADD });
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
        public bool IsNameCityAvailble(string Name, CityViewModel City)
        {
            try
            {
                bool IsOrder = !_repository.CityRepository.Any(cc => cc.Name.Trim().ToLower() == Name.Trim().ToLower() && cc.ID != City.ID);
                return IsOrder;
            }
            catch (Exception)
            {
                return true;
            }
        }
        public bool IsCityExists(CityViewModel City)
        {
            try
            {
                bool IsOrder = _repository.CityRepository.Any(cc => cc.ID == City.ID);
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