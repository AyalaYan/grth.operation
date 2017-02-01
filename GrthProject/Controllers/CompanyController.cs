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
    public class CompanyController : ApiController
    {
        #region Private/protected fields
        protected readonly RepositoryContainer _repository = new RepositoryContainer();
        #endregion

        #region Api Methods

        // GET api/company
        [Route("api/company/{sort}")]
        public IHttpActionResult Get([FromBody]string sort)
        {
            IEnumerable<CompanyView> company = null;
            try
            {
                int companyCount;
                 company = _repository.CompanyRepository.GetByFiltering(0, 0, null, null, out companyCount);
            }
            catch { }

            return Json(new { Records = company });
        }
        // GET api/company/options
        [Route("api/company")]
        public IHttpActionResult Get()
        {
            List<Options> Options = null;
            try
            {
                Options = _repository.CompanyRepository.GetOptionsNoAsync(opt => new Options() { DisplayText = opt.Name, Value = opt.ID }, opt => opt.IsActive == true);
            }
            catch { }
            return Json(new { Options = Options });
        }
        // POST api/company
        public IHttpActionResult Post([FromBody]CompanyViewModel CompanyVM)
        {
            //Update or Create
            try
            {
                if (IscompanyExists(CompanyVM))
                {
                    return Insertcompany(CompanyVM);
                }
                else
                {
                    return Updatecompany(CompanyVM);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }
        }
        // POST api/company/5
        public IHttpActionResult Post(int ID)
        {
            try
            {
                var ObjDel = _repository.CompanyRepository.GetField(ID);
                if (!ObjDel.IsAllowDelete)
                    return Json(new { Result = "ERROR", Message = Errors.MessageErrorAllowDelete });
                else
                {
                    Company companyDel = _repository.CompanyRepository.Find(ID);
                    _repository.CompanyRepository.Delete(companyDel);
                    _repository.CompanyRepository.Save();
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

        private IHttpActionResult Updatecompany(CompanyViewModel CompanyVM)
        {
            try
            {
                if (!IsNamecompanyAvailble(CompanyVM.Name, CompanyVM))
                    return Json(new { Result = "ERROR", Message = "company Name already exists" });
                Company CompanyUpdated = _repository.CompanyRepository.Find(CompanyVM.ID);
                CompanyUpdated.Name = CompanyVM.Name;
                CompanyUpdated.CustomerID = CompanyVM.CustomerID;
                CompanyUpdated.IsActive = CompanyVM.IsActive;
                _repository.CompanyRepository.Edit(CompanyUpdated);
                if (_repository.CompanyRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.CompanyRepository.GetField(CompanyVM.ID);
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

        private IHttpActionResult Insertcompany(CompanyViewModel CompanyVM)
        {
            try
            {
                if (!IsNamecompanyAvailble(CompanyVM.Name, CompanyVM))
                    return Json(new { Result = "ERROR", Message = "company Name already exists" });
                Company CompanyADD = new Company { Name = CompanyVM.Name, CustomerID = CompanyVM.CustomerID, IsActive = CompanyVM.IsActive };
                _repository.CompanyRepository.Add(CompanyADD);
                if (_repository.CompanyRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.CompanyRepository.GetField(CompanyADD.ID);
                    return Json(new { Result = "OK", Record = CompanyADD });
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
        public bool IsNamecompanyAvailble(string Name, CompanyViewModel company)
        {
            try
            {
                bool IsOrder = !_repository.CompanyRepository.Any(cc => cc.Name.Trim().ToLower() == Name.Trim().ToLower() && cc.ID != company.ID);
                return IsOrder;
            }
            catch (Exception)
            {
                return true;
            }
        }
        public bool IscompanyExists(CompanyViewModel company)
        {
            try
            {
                bool IsOrder = _repository.CompanyRepository.Any(cc => cc.ID == company.ID);
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