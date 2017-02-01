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
    public class JobsController : ApiController
    {
        #region Private/protected fields
        protected readonly RepositoryContainer _repository = new RepositoryContainer();
        #endregion

        #region Api Methods

        // GET api/jobs
        [Route("api/jobs/{sort}")]
        public IHttpActionResult Get([FromBody]string sort)
        {
            IEnumerable<JobView> jobs = null;
            try
            {
                int JobCount;
                jobs = _repository.JobRepository.GetByFiltering(0, 0, null, null, out JobCount);
                return Json(new { Records = jobs });
            }
            catch
            {
                return Json(new { Records = jobs });
            }
        }
        // GET api/jobs/
        [Route("api/jobs")]
        public IHttpActionResult Get()
        {
            List<Options> Options = null;
            try
            {
                Options = _repository.JobRepository.GetOptionsNoAsync(opt => new Options() { DisplayText = opt.Name, Value = opt.ID }, opt => opt.IsActive == true);
            }
            catch { }
            return Json(new { Options = Options });
        }
        // POST api/jobs

        [HttpPost]
        public IHttpActionResult Post([FromBody]JobViewModel JobVM)
        {
            //Update or Create
            try
            {
                if (!IsJobExists(JobVM.ID))
                {
                    return InsertJob(JobVM);
                }
                else
                {
                    return UpdateJob(JobVM);

                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = Errors.Write(ex) });
            }
        }
        // POST api/jobs/5
        public IHttpActionResult Post(int id)
        {
            try
            {
                var ObjDel = _repository.JobRepository.GetField(id);
                if (!ObjDel.IsAllowDelete)
                    return Json(new { Result = "ERROR", Message = Errors.MessageErrorAllowDelete });
                else
                {
                    Job JobDel = _repository.JobRepository.Find(id);
                    _repository.JobRepository.Delete(JobDel);
                    _repository.JobRepository.Save((WindowsPrincipal)User);
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

        private IHttpActionResult UpdateJob(JobViewModel JobVM)
        {
            try
            {
                if (!IsNameJobAvailble(JobVM.Name, JobVM))
                    return Json(new { Result = "ERROR", Message = "Job Name already exists" });
                Job JobUpdated = _repository.JobRepository.Find(JobVM.ID);
                JobUpdated.Name = JobVM.Name;
                JobUpdated.IsActive = JobVM.IsActive;
                _repository.JobRepository.Edit(JobUpdated);
                if (_repository.JobRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.JobRepository.GetField(JobVM.ID); ;
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

        private IHttpActionResult InsertJob(JobViewModel JobVM)
        {
            try
            {
                if (!IsNameJobAvailble(JobVM.Name, JobVM))
                    return Json(new { Result = "ERROR", Message = "Job Name already exists" });
                Job JobADD = new Job { Name = JobVM.Name, IsActive = JobVM.IsActive };

                _repository.JobRepository.Add(JobADD);
                if (_repository.JobRepository.Save((WindowsPrincipal)User) > 0)
                {
                    var Record = _repository.JobRepository.GetField(JobADD.ID); ;
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
        private bool IsNameJobAvailble(string Name,JobViewModel Job)
        {
            try
            {
                bool IsOrder = !_repository.JobRepository.Any(cc => cc.Name.Trim().ToLower() == Name.Trim().ToLower() && cc.ID != Job.ID);
                return IsOrder;
            }
            catch (Exception)
            {
                return true;
            }
        }
        private bool IsJobExists(int iJobId)
        {
            try
            {
                bool IsOrder = _repository.JobRepository.Any(cc => cc.ID == iJobId);
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