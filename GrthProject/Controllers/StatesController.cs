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
    public class StatesController : ApiController
    {
        #region Private/protected fields
        protected readonly RepositoryContainer _repository = new RepositoryContainer();
        #endregion

        #region Api Methods

        // GET api/states
        [Route("api/states/{sort}")]
        public IHttpActionResult Get([FromBody]string sort)
        {
            IEnumerable<StateView> statesList = null;
            try
            {
                int StateCount;
                statesList = _repository.StateRepository.GetByFiltering(0, 0, null, null, out StateCount);
            }
            catch { }

            return Json(new { Records = statesList });
        }
        // GET api/states/iCountryID
        [Route("api/states/{iCountryID}")]
        public IHttpActionResult Get([FromUri]int iCountryID)
        {
            List<Options> Options = null;
            try
            {
                Options = _repository.StateRepository.GetOptionsNoAsync(opt => new Options()
                { DisplayText = opt.Name, Value = opt.ID }, opt => opt.IsActive == true && opt.CountryID == iCountryID);
            }
            catch { }
            return Json(new { Options = Options });
        }
        // POST api/states
        public IHttpActionResult Post([FromBody]StateViewModel StateVM)
        {
            //Update or Create
            try
            {
                if (IsStateExists(StateVM))
                {
                    return InsertState(StateVM);
                }
                else
                {
                    return UpdateState(StateVM);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }
        }
        // POST api/states/5
        public IHttpActionResult Post(int ID)
        {
            try
            {
                var ObjDel = _repository.StateRepository.GetField(ID);
                if (!ObjDel.IsAllowDelete)
                    return Json(new { Result = "ERROR", Message = Errors.MessageErrorAllowDelete });
                else
                {
                    State StateDel = _repository.StateRepository.Find(ID);
                    _repository.StateRepository.Delete(StateDel);
                    _repository.StateRepository.Save((WindowsPrincipal)User);
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

        private IHttpActionResult UpdateState(StateViewModel StateVM)
        {
            try
            {
                if (!IsNameStateAvailble(StateVM.Name, StateVM))
                    return Json(new { Result = "ERROR", Message = "State Name already exists" });
                State StateUpdated = _repository.StateRepository.Find(StateVM.ID);
                StateUpdated.Name = StateVM.Name;
                StateUpdated.IsActive = StateVM.IsActive;
                StateUpdated.CountryID = StateVM.CountryID;
                _repository.StateRepository.Edit(StateUpdated);
                if (_repository.StateRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.StateRepository.GetField(StateVM.ID);
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

        private IHttpActionResult InsertState(StateViewModel StateVM)
        {
            try
            {
                if (!IsNameStateAvailble(StateVM.Name, StateVM))
                    return Json(new { Result = "ERROR", Message = "State Name already exists" });
                State StateADD = new State
                {
                    Name = StateVM.Name,
                    IsActive = StateVM.IsActive,
                    CountryID = StateVM.CountryID,
                    IsSystem = false
                };
                _repository.StateRepository.Add(StateADD);
                if (_repository.StateRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.StateRepository.GetField(StateADD.ID);
                    return Json(new { Result = "OK", Record = StateADD });
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
        public bool IsNameStateAvailble(string Name, StateViewModel State)
        {
            try
            {
                bool IsOrder = !_repository.StateRepository.Any(cc => cc.Name.Trim().ToLower() == Name.Trim().ToLower() && cc.ID != State.ID);
                return IsOrder;
            }
            catch (Exception)
            {
                return true;
            }
        }
        public bool IsStateExists(StateViewModel State)
        {
            try
            {
                bool IsOrder = _repository.StateRepository.Any(cc => cc.ID == State.ID);
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