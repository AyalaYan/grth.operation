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
    public class CountriesController : ApiController
    {
        #region Private/protected fields
        protected readonly RepositoryContainer _repository = new RepositoryContainer();
        #endregion

        #region Api Methods

        // GET api/countries
        [Route("api/countries/{sort}")]
        public IHttpActionResult Get([FromBody]string sort)
        {
            IEnumerable<CountryView> countries = null;
            try
            {
                int CountryCount;
                var Countries = _repository.CountryRepository.GetByFiltering(0, 0, null, null, out CountryCount);
            }
            catch { }

            return Json(new { Records = countries });
        }
        // GET api/countries/options
        [Route("api/countries")]
        public IHttpActionResult Get()
        {
            List<Options> Options = null;
            try
            {
                 Options =  _repository.CountryRepository.GetOptionsNoAsync(opt => new Options() { DisplayText = opt.Name, Value = opt.ID }, opt => opt.IsActive == true);
            }
            catch { }
            return Json(new { Options = Options });
        }
        // POST api/countries
        public IHttpActionResult Post([FromBody]CountryViewModel CountryVM)
        {
            //Update or Create
            try
            {
                if (IsCountryExists(CountryVM))
                {
                    return  InsertCountry(CountryVM);
                }
                else
                {
                    return  UpdateCountry(CountryVM);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }
        }
        // POST api/countries/5
        public IHttpActionResult Post(int ID)
        {
            try
            {
                var ObjDel = _repository.CountryRepository.GetField(ID);
                if (!ObjDel.IsAllowDelete)
                    return Json(new { Result = "ERROR", Message = Errors.MessageErrorAllowDelete });
                else
                {
                    Country CountryDel = _repository.CountryRepository.Find(ID);
                    _repository.CountryRepository.Delete(CountryDel);
                     _repository.CountryRepository.Save();
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

        private IHttpActionResult UpdateCountry(CountryViewModel CountryVM)
        {
            try
            {
                if (!IsNameCountryAvailble(CountryVM.Name, CountryVM))
                    return Json(new { Result = "ERROR", Message = "Country Name already exists" });
                Country CountryUpdated = _repository.CountryRepository.Find(CountryVM.ID);
                CountryUpdated.Name = CountryVM.Name;
                CountryUpdated.ShortName = CountryVM.ShortName;
                CountryUpdated.IsActive = CountryVM.IsActive;
                _repository.CountryRepository.Edit(CountryUpdated);
                if ( _repository.CountryRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.CountryRepository.GetField(CountryVM.ID);
                    return Json(new { Result = "OK", Record = Record });
                }
                else
                    return Json(new { Result = "ERROR", Message = Errors.MessageError });
            }
            catch {
                return Json(new { Result = "ERROR", Message = Errors.MessageError });
            }

        }

        private IHttpActionResult InsertCountry(CountryViewModel CountryVM)
        {
            try
            {
                if (!IsNameCountryAvailble(CountryVM.Name, CountryVM))
                    return Json(new { Result = "ERROR", Message = "Country Name already exists" });
                Country CountryADD =
                 new Country
                 {
                     Name = CountryVM.Name,
                     ShortName = CountryVM.ShortName,
                     IsSystem = false,
                     IsActive = CountryVM.IsActive
                 };
                _repository.CountryRepository.Add(CountryADD);
                if ( _repository.CountryRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.CountryRepository.GetField(CountryADD.ID);
                    return Json(new { Result = "OK", Record = CountryADD });
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
        public bool IsNameCountryAvailble(string Name,  CountryViewModel Country)
        {
            try
            {
                bool IsOrder = !_repository.CountryRepository.Any(cc => cc.Name.Trim().ToLower() == Name.Trim().ToLower() && cc.ID != Country.ID);
                return IsOrder;
            }
            catch (Exception)
            {
                return true;
            }
        }
        public bool IsCountryExists(CountryViewModel Country)
        {
            try
            {
                bool IsOrder = _repository.CountryRepository.Any(cc => cc.ID == Country.ID);
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








