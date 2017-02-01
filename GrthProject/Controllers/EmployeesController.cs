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
    public class EmployeesController : ApiController
    {
        #region Private/protected fields
        protected readonly RepositoryContainer _repository = new RepositoryContainer();
        #endregion

        #region Api Methods

        // GET api/employees
        public IHttpActionResult Get()
        {
            IEnumerable<EmployeeView> Employees = null;
            try
            {
                int EmployeeCount;
                Employees = _repository.EmployeeRepository.GetByFiltering(0, 0, null, null, out EmployeeCount);
                return Json(new { Records = Employees });
            }
            catch
            {
                return Json(new { Records = Employees });
            }
        }
        // GET api/employees/options
        public IHttpActionResult Get(bool options)
        {
            List<Options> Options = null;
            try
            {
                Options = _repository.EmployeeRepository.GetOptionsNoAsync(opt => new Options() { DisplayText = opt.FirstName + " " + opt.LastName, Value = opt.ID }, opt => opt.IsActive == true);
            }
            catch { }
            return Json(new { Options = Options });
        }
        // POST api/employees

        [HttpPost]
        public IHttpActionResult Post([FromBody]EmployeeViewModel EmployeeVM)
        {
            //Update or Create
            try
            {
                if (!IsEmployeeExists(EmployeeVM.ID))
                {
                    return InsertEmployee(EmployeeVM);
                }
                else
                {
                    return UpdateEmployee(EmployeeVM);

                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }
        }
        // POST api/employees/5
        public IHttpActionResult Post(int id)
        {
            try
            {
                var ObjDel = _repository.EmployeeRepository.GetField(id);
                if (!ObjDel.IsAllowDelete)
                    return Json(new { Result = "ERROR", Message = Errors.MessageErrorAllowDelete });
                else
                {
                    Employee EmployeeDel = _repository.EmployeeRepository.Find(id);
                    _repository.EmployeeRepository.Delete(EmployeeDel);
                    _repository.EmployeeRepository.Save((WindowsPrincipal)User);
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

        private IHttpActionResult UpdateEmployee(EmployeeViewModel EmployeeVM)
        {
            try
            {
                if (!IsNameEmployeeAvailble(EmployeeVM.FirstName, EmployeeVM.LastName, EmployeeVM))
                    return Json(new { Result = "ERROR", Message = "Employee Name already exists" });
                Employee EmployeeUpdated = _repository.EmployeeRepository.Find(EmployeeVM.ID);
                EmployeeUpdated.FirstName = EmployeeVM.FirstName;
                EmployeeUpdated.LastName = EmployeeVM.LastName;
                EmployeeUpdated.FirstFamilyName = EmployeeVM.FirstFamilyName;
                EmployeeUpdated.StartDate = EmployeeVM.StartDate;
                EmployeeUpdated.Address = EmployeeVM.Address;
                EmployeeUpdated.PhoneNumber = EmployeeVM.PhoneNumber;
                EmployeeUpdated.Remarks = EmployeeVM.Remarks;
                EmployeeUpdated.JobID = EmployeeVM.JobID;
                EmployeeUpdated.CountryID = EmployeeVM.CountryID;
                EmployeeUpdated.StateID = EmployeeVM.StateID;
                EmployeeUpdated.CityID = EmployeeVM.CityID;
                EmployeeUpdated.IsActive = EmployeeVM.IsActive;
                _repository.EmployeeRepository.Edit(EmployeeUpdated);
                if (_repository.EmployeeRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.EmployeeRepository.GetField(EmployeeVM.ID); ;
                    return Json(new { Result = "OK", Record = Record });
                }
                else
                    return Json(new { Result = "ERROR", Message = Errors.MessageError });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }

        }

        private IHttpActionResult InsertEmployee(EmployeeViewModel EmployeeVM)
        {
            try
            {
                if (!IsNameEmployeeAvailble(EmployeeVM.FirstName, EmployeeVM.LastName, EmployeeVM))
                    return Json(new { Result = "ERROR", Message = "Employee Name already exists" });

                Employee EmployeeADD = new Employee
                {
                    FirstName = EmployeeVM.FirstName,
                    LastName = EmployeeVM.LastName,
                    FirstFamilyName = EmployeeVM.FirstFamilyName,
                    Address = EmployeeVM.Address,
                    PhoneNumber = EmployeeVM.PhoneNumber,
                    Remarks = EmployeeVM.Remarks,
                    JobID = EmployeeVM.JobID,
                    CountryID = EmployeeVM.CountryID,
                    StateID = EmployeeVM.StateID,
                    CityID = EmployeeVM.CityID,
                    IsActive = EmployeeVM.IsActive
                };
                _repository.EmployeeRepository.Add(EmployeeADD);
                if (_repository.EmployeeRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.EmployeeRepository.GetField(EmployeeADD.ID); ;
                    return Json(new { Result = "OK", Record = Record });
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
        private bool IsNameEmployeeAvailble(string FirstName, string LastName, EmployeeViewModel Employee)
        {
            try
            {
                bool IsOrder = !_repository.EmployeeRepository.Any(cc => cc.FirstName.Trim().ToLower() == FirstName.Trim().ToLower() && cc.LastName.Trim().ToLower() == LastName.Trim().ToLower() && cc.ID != Employee.ID);
                return IsOrder;
            }
            catch (Exception)
            {
                return true;
            }
        }
        private bool IsEmployeeExists(int iEmployeeId)
        {
            try
            {
                bool IsOrder = _repository.EmployeeRepository.Any(cc => cc.ID == iEmployeeId);
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