using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeAdmin.Data;
using SafeAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeAdmin.Controllers
{
    [Route("patients")]
    public class ApiPatientController : Controller
    {
        private readonly SafeContext _context;

        public ApiPatientController(SafeContext context)
        {
            _context = context;
        }

        // GET /patients
        /// <summary>
        /// Get all patients
        /// </summary>
        [HttpGet]
        public IEnumerable<Patient> Get()
        {
            var patients = _context.Patient.Include(patient => patient.Person).ThenInclude(person => person.addresses);
            return patients;
            //return _context.Patient;
        }

        // GET /patients/5
        /// <summary>
        /// Get patient by Id
        /// </summary>
        [HttpGet("{id}")]
        public Patient Get(int id)
        {
            return _context.Patient.Include(patient => patient.Person).ThenInclude(person => person.addresses).FirstOrDefault(patient => patient.ID == id);
        }

        // POST /patients
        /// <summary>
        /// Create patient
        /// </summary>
        [HttpPost]
        public Patient Post([FromBody]Patient patient)
        {
            _context.Patient.Add(patient);
            _context.SaveChanges();
            return patient;
        }

        // PUT /patients/5
        /// <summary>
        /// Update patient
        /// </summary>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Patient patient)
        {
            //todo: make use of id and validation
            _context.Patient.Update(patient);
            _context.SaveChanges();
        }

        // DELETE /patients/5
        /// <summary>
        /// Delete patient
        /// </summary>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _context.Patient.Remove(_context.Patient.FirstOrDefault(p => p.ID == id));
            _context.SaveChanges();
        }


    }
}
