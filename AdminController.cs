using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using SafeAdmin.Model;
using SafeAdmin.Data;
using Newtonsoft.Json;
using SafeAdmin.Services;

namespace SafeAdmin.Controllers
{
    public class AdminController : Controller
    {
        private readonly SafeContext _context;
        private readonly IFindLocation _findLocation;

        public AdminController(SafeContext context, IFindLocation findLocation)
        {
            _context = context;
            _findLocation = findLocation;
        }


        public IActionResult Index()
        {
            return View();
        }


        #region Members
        public async Task<IActionResult> Members()
        {
            return View(await _context.Member.ToListAsync());
        }

        // GET: Members/Create
        public IActionResult CreateMember()
        {
            ViewData["Message"] = "Create new member.";
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMember([Bind("FirstName,LastName,Email,MobileNumber,State,Zip,BirthDate,Sex,IsActive")] Member member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(member);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Members");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> EditMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.SingleOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMember(int id, [Bind("ID,BirthDate,Email,FirstName,IsActive,LastName,MobileNumber,Sex,State,Zip")] Member member)
        {
            if (id != member.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Members");
            }
            return View(member);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> MemberDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.SingleOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> DeleteMember(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("DeleteMemberConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMemberConfirmed(int id)
        {
            var member = await _context.Member.SingleOrDefaultAsync(m => m.ID == id);
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Members");
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.ID == id);
        }

        #endregion

        #region Facilities

        public async Task<IActionResult> Facilities(int? page)
        {
            int pageSize = 10;
            var facilities = _context.Facility;
            return View(await PaginatedList<Facility>.CreateAsync(facilities.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Facility/Create
        public IActionResult CreateFacility()
        {
            ViewData["Message"] = "Create new facility.";
            return View();
        }

        // POST: Facility/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFacility([Bind("Key,Country,Name,Address1,Address2,City,State,Zip,Phone,Doctor,Website,ImageUrl,IsActive")] Facility facility)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region find cordinates
                    decimal[] location = _findLocation.FindAdressLocation(facility.Country,facility.State, facility.City, facility.Zip, facility.Address1);
                    facility.Latitude = location[0];
                    facility.Longitude = location[1];
                    #endregion
                    _context.Add(facility);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Facilities");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(facility);
        }

        // GET: Facility/Edit/5
        public async Task<IActionResult> EditFacility(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facility = await _context.Facility.SingleOrDefaultAsync(f => f.ID == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFacility(int id, [Bind("ID,Unknown1,Unknown2,CountryCode,Unknown3,Unknown4,Name,Address1,Address2,City,State,Zip,Phone,Unknown5,Unknown6,Unknown7,Unknown8,Unknown9,Unknown10,Unknown11,Physician,LocationSlug,Lat,Long,Uid,Npi,Website.Description,AcceptsNewPatients,ImageUrl,IsActive")] Facility facility)
        {
            if (id != facility.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityExists(facility.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Facilities");
            }
            return View(facility);
        }

        // GET: Facility/Details/5
        public async Task<IActionResult> FacilityDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facility = await _context.Facility.SingleOrDefaultAsync(f => f.ID == id);
            if (facility.Latitude == 0 && facility.Longitude == 0)
            {
                #region find cordinates
                decimal[] location = _findLocation.FindAdressLocation(facility.Country, facility.State, facility.City, facility.Zip, facility.Address1);
                facility.Latitude = location[0];
                facility.Longitude = location[1];
                #endregion
                _context.Update(facility);
                _context.SaveChanges();
            }
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // GET: Facility/Delete/5
        public async Task<IActionResult> DeleteFacility(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facility = await _context.Facility
                .AsNoTracking()
                .SingleOrDefaultAsync(f => f.ID == id);
            if (facility == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(facility);
        }

        // POST: Facility/Delete/5
        [HttpPost, ActionName("DeleteFacilityConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFacilityConfirmed(int id)
        {
            var facility = await _context.Facility.SingleOrDefaultAsync(m => m.ID == id);
            _context.Facility.Remove(facility);
            await _context.SaveChangesAsync();
            return RedirectToAction("Facilities");
        }

        private bool FacilityExists(int id)
        {
            return _context.Facility.Any(e => e.ID == id);
        }

        #endregion

        #region Patients
        public async Task<IActionResult> Patients()
        {
            return View(await _context.Patient.Include(patient => patient.Person).ThenInclude(person => person.addresses).ToListAsync());
        }

        // GET: Members/Create
        public IActionResult CreatePatient()
        {
            ViewData["Message"] = "Create new patient.";
            return View();
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> EditPatient(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.SingleOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> DeletePatient(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("DeleteMemberConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePatientConfirmed(int id)
        {
            var member = await _context.Member.SingleOrDefaultAsync(m => m.ID == id);
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Members");
        }

        private bool PatientExists(int id)
        {
            return _context.Member.Any(e => e.ID == id);
        }

        #endregion

        #region MaritalStatus
        public async Task<IActionResult> MaritalStatuses()
        {
            return View(await _context.MaritalStatus.ToListAsync());
        }

        // POST: MeritalStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMaritalStatus([Bind("Value")] MaritalStatus maritalStatus)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(maritalStatus);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("MaritalStatuses");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(maritalStatus);
        }

        // GET: MeritalStatus/Create
        public IActionResult CreateMaritalStatus()
        {
            ViewData["Message"] = "Create new Marital Status.";
            return View();
        }

        // GET: MeritalStatus/Delete/5
        public async Task<IActionResult> DeleteMaritalStatus(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maritalStatus = await _context.MaritalStatus
                .AsNoTracking()
                .SingleOrDefaultAsync(l => l.ID == id);
            if (maritalStatus == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(maritalStatus);
        }

        // POST: MeritalStatus/Delete/5
        [HttpPost, ActionName("DeleteMaritalStatusConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMaritalStatusConfirmed(int id)
        {
            var maritalStatus = await _context.MaritalStatus.SingleOrDefaultAsync(e => e.ID == id);
            _context.MaritalStatus.Remove(maritalStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction("MeritalStatuses");
        }

        // GET: MeritalStatus/Edit/5
        public async Task<IActionResult> EditMaritalStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maritalStatus = await _context.MaritalStatus.SingleOrDefaultAsync(e => e.ID == id);
            if (maritalStatus == null)
            {
                return NotFound();
            }

            return View(maritalStatus);
        }

        // POST: MeritalStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMaritalStatus(int id, [Bind("ID", "Value")] MaritalStatus maritalStatus)
        {
            if (id != maritalStatus.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maritalStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeritalStatusExists(maritalStatus.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MaritalStatuses");
            }
            return View(maritalStatus);
        }

        private bool MeritalStatusExists(int id)
        {
            return _context.MaritalStatus.Any(e => e.ID == id);
        }
        #endregion

        #region Ethnicity
        public async Task<IActionResult> Ethnicities()
        {
            return View(await _context.Ethnicity.ToListAsync());
        }

        // POST: Ethnicity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEthnicity([Bind("Value")] Ethnicity ethnicity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(ethnicity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Ethnicities");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(ethnicity);
        }

        // GET: Ethnicity/Create
        public IActionResult CreateEthnicity()
        {
            ViewData["Message"] = "Create new ethnicity.";
            return View();
        }

        // GET: Ethnicity/Delete/5
        public async Task<IActionResult> DeleteEthnicity(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ethnicity = await _context.Ethnicity
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.ID == id);
            if (ethnicity == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(ethnicity);
        }

        // POST: Ethnicity/Delete/5
        [HttpPost, ActionName("DeleteEthnicityConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEthnicityConfirmed(int id)
        {
            var ethnicity = await _context.Ethnicity.SingleOrDefaultAsync(e => e.ID == id);
            _context.Ethnicity.Remove(ethnicity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Ethnicities");
        }

        // GET: Ethnicity/Edit/5
        public async Task<IActionResult> EditEthnicity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ethnicity = await _context.Ethnicity.SingleOrDefaultAsync(e => e.ID == id);
            if (ethnicity == null)
            {
                return NotFound();
            }

            return View(ethnicity);
        }

        // POST: Ethnicity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEthnicity(int id, [Bind("ID","Value")] Ethnicity ethnicity)
        {
            if (id != ethnicity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ethnicity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EthnicityExists(ethnicity.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Ethnicities");
            }
            return View(ethnicity);
        }

        private bool EthnicityExists(int id)
        {
            return _context.Ethnicity.Any(e => e.ID == id);
        }
        #endregion

        #region Language
        public async Task<IActionResult> Languages()
        {
            return View(await _context.Language.ToListAsync());
        }

        // POST: Language/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLanguage([Bind("Value")] Language language)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(language);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Languages");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(language);
        }

        // GET: Language/Create
        public IActionResult CreateLanguage()
        {
            ViewData["Message"] = "Create new language.";
            return View();
        }

        // GET: Language/Delete/5
        public async Task<IActionResult> DeleteLanguage(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Language
                .AsNoTracking()
                .SingleOrDefaultAsync(l => l.ID == id);
            if (language == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(language);
        }

        // POST: Language/Delete/5
        [HttpPost, ActionName("DeleteLanguageConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLanguageConfirmed(int id)
        {
            var language = await _context.Language.SingleOrDefaultAsync(e => e.ID == id);
            _context.Language.Remove(language);
            await _context.SaveChangesAsync();
            return RedirectToAction("Languages");
        }

        // GET: Language/Edit/5
        public async Task<IActionResult> EditLanguage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Language.SingleOrDefaultAsync(e => e.ID == id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // POST: Language/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLanguage(int id, [Bind("ID", "Value")] Language language)
        {
            if (id != language.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(language);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(language.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Languages");
            }
            return View(language);
        }

        private bool LanguageExists(int id)
        {
            return _context.Ethnicity.Any(e => e.ID == id);
        }
        #endregion

        #region RaceOption
        public async Task<IActionResult> RaceOptions()
        {
            return View(await _context.RaceOption.ToListAsync());
        }

        // POST: RaceOption/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRaceOption([Bind("HasValue","Value")] RaceOption raceOption)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(raceOption);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("RaceOptions");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(raceOption);
        }

        // GET: RaceOption/Create
        public IActionResult CreateRaceOption()
        {
            ViewData["Message"] = "Create new race option.";
            return View();
        }

        // GET: RaceOption/Delete/5
        public async Task<IActionResult> DeleteRaceOption(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceOption = await _context.RaceOption
                .AsNoTracking()
                .SingleOrDefaultAsync(l => l.ID == id);
            if (raceOption == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(raceOption);
        }

        // POST: RaceOption/Delete/5
        [HttpPost, ActionName("DeleteRaceOptionConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRaceOptionConfirmed(int id)
        {
            var raceOption = await _context.RaceOption.SingleOrDefaultAsync(e => e.ID == id);
            _context.RaceOption.Remove(raceOption);
            await _context.SaveChangesAsync();
            return RedirectToAction("RaceOptions");
        }

        // GET: RaceOption/Edit/5
        public async Task<IActionResult> EditRaceOption(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceOption = await _context.RaceOption.SingleOrDefaultAsync(e => e.ID == id);
            if (raceOption == null)
            {
                return NotFound();
            }

            return View(raceOption);
        }

        // POST: RaceOption/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRaceOption(int id, [Bind("ID", "HasValue", "Value")] RaceOption raceOption)
        {
            if (id != raceOption.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceOptionExists(raceOption.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("RaceOptions");
            }
            return View(raceOption);
        }

        private bool RaceOptionExists(int id)
        {
            return _context.RaceOption.Any(e => e.ID == id);
        }
        #endregion

        #region Race
        public async Task<IActionResult> Races()
        {
            return View(await _context.Race.ToListAsync());
        }

        // POST: Race/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRace([Bind("Value")] Race race)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(race);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Races");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(race);
        }

        // GET: Race/Create
        public IActionResult CreateRace()
        {
            ViewData["Message"] = "Create new race.";
            return View();
        }

        // GET: Race/Delete/5
        public async Task<IActionResult> DeleteRace(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Race
                .AsNoTracking()
                .SingleOrDefaultAsync(l => l.ID == id);
            if (race == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(race);
        }

        // POST: Race/Delete/5
        [HttpPost, ActionName("DeleteRaceConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRaceConfirmed(int id)
        {
            var race = await _context.Race.SingleOrDefaultAsync(e => e.ID == id);
            _context.Race.Remove(race);
            await _context.SaveChangesAsync();
            return RedirectToAction("Races");
        }

        // GET: Race/Edit/5
        public async Task<IActionResult> EditRace(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Race.SingleOrDefaultAsync(e => e.ID == id);
            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }

        // POST: Race/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRace(int id, [Bind("ID", "Value")] Race race)
        {
            if (id != race.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(race);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceExists(race.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Races");
            }
            return View(race);
        }

        private bool RaceExists(int id)
        {
            return _context.Race.Any(e => e.ID == id);
        }
        #endregion

        #region Sex
        public async Task<IActionResult> Sexes()
        {
            return View(await _context.Sex.ToListAsync());
        }

        // POST: Sex/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSex([Bind("Value")] Sex sex)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(sex);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Sexes");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(sex);
        }

        // GET: Sex/Create
        public IActionResult CreateSex()
        {
            ViewData["Message"] = "Create new sex.";
            return View();
        }

        // GET: Sex/Delete/5
        public async Task<IActionResult> DeleteSex(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sex = await _context.Sex
                .AsNoTracking()
                .SingleOrDefaultAsync(l => l.ID == id);
            if (sex == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(sex);
        }

        // POST: Sex/Delete/5
        [HttpPost, ActionName("DeleteSexConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSexConfirmed(int id)
        {
            var sex = await _context.Sex.SingleOrDefaultAsync(e => e.ID == id);
            _context.Sex.Remove(sex);
            await _context.SaveChangesAsync();
            return RedirectToAction("Sexes");
        }

        // GET: Sex/Edit/5
        public async Task<IActionResult> EditSex(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sex = await _context.Sex.SingleOrDefaultAsync(e => e.ID == id);
            if (sex == null)
            {
                return NotFound();
            }

            return View(sex);
        }

        // POST: Sex/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSex(int id, [Bind("ID", "Value")] Sex sex)
        {
            if (id != sex.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sex);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SexExists(sex.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Sexes");
            }
            return View(sex);
        }

        private bool SexExists(int id)
        {
            return _context.Sex.Any(e => e.ID == id);
        }
        #endregion

        #region PayerType
        public async Task<IActionResult> PayerTypes()
        {
            return View(await _context.PayerType.ToListAsync());
        }

        // POST: PayerType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePayerType([Bind("Value")] PayerType payerType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(payerType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("PayerTypes");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(payerType);
        }

        // GET: PayerType/Create
        public IActionResult CreatePayerType()
        {
            ViewData["Message"] = "Create new Payer Type.";
            return View();
        }

        // GET: PayerType/Delete/5
        public async Task<IActionResult> DeletePayerType(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payerType = await _context.PayerType
                .AsNoTracking()
                .SingleOrDefaultAsync(l => l.ID == id);
            if (payerType == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(payerType);
        }

        // POST: PayerType/Delete/5
        [HttpPost, ActionName("DeletePayerTypeConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePayerTypeConfirmed(int id)
        {
            var payerType = await _context.PayerType.SingleOrDefaultAsync(e => e.ID == id);
            _context.PayerType.Remove(payerType);
            await _context.SaveChangesAsync();
            return RedirectToAction("PayerTypes");
        }

        // GET: PayerType/Edit/5
        public async Task<IActionResult> EditPayerType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payerType = await _context.PayerType.SingleOrDefaultAsync(e => e.ID == id);
            if (payerType == null)
            {
                return NotFound();
            }

            return View(payerType);
        }

        // POST: PayerType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPayerType(int id, [Bind("ID", "Value")] PayerType payerType)
        {
            if (id != payerType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payerType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayerTypeExists(payerType.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("PayerTypes");
            }
            return View(payerType);
        }

        private bool PayerTypeExists(int id)
        {
            return _context.PayerType.Any(e => e.ID == id);
        }
        #endregion
    }
}
