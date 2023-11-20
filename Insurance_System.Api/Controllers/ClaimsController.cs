using Insurance_System.Domain.Service;
using Insurance_System.Domain;
using Insurance_System.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Insurance_System.Domain.Domain;
using Insurance_System.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using System.Linq;

namespace Insurance_System.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        
       private readonly GenericServiceAsync<ClaimModel, Claim>  _genericServiceAsync;
        public ClaimsController(GenericServiceAsync<ClaimModel, Claim> genericServiceAsync)
        {
                _genericServiceAsync = genericServiceAsync;
        }
        // GET: api/claims
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Claim))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Claim>>> GetClaims()
        {
            var Claims= await _genericServiceAsync.GetAll();
           return Ok(Claims); 
        }
        // POST: api/claims
        [HttpPost]
        public async Task<ActionResult<Claim>> PostClaim(ClaimModel claim)
        {
            await _genericServiceAsync.Add(claim);
          
            return CreatedAtAction(nameof(Claim), new { id = claim.Id }, claim);
        }
        // PUT: api/claims/{id}
        [Authorize(Roles = "Administrator,ClaimProcessors")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaim(int id, ClaimModel claim)
        {
            if (id != claim.Id)
            {
                return BadRequest();
            }
            try
            {
                int retVal = await _genericServiceAsync.Update(claim);
                if (retVal == 0)
                    return StatusCode(304);  //Not Modified
                else if (retVal == -1)
                    return StatusCode(412, "DbUpdateConcurrencyException");  //412 Precondition Failed  - concurrency
                else
                    return Accepted(claim);
            }
            catch (DbUpdateConcurrencyException)
            {
                var checkClaimExists = await ClaimExists(id);
                if (!checkClaimExists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE: api/claims/{id}
        [Authorize(Roles = "Administrator,ClaimProcessors")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaim(int id)
        {
            int retVal = await _genericServiceAsync.Remove(id);
            if (retVal == 0)
                return NotFound();  //Not Found 404
            else if (retVal == -1)
                return StatusCode(412, "DbUpdateConcurrencyException");  //Precondition Failed  - concurrency
            else
                return NoContent();   	     //No Content 204
        }
        [Authorize(Roles = "Administrator,ClaimProcessors")]
        // GET: api/claims/pending
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Claim>>> GetPendingClaims()
        {
            var pendingClaims = await _genericServiceAsync
                .Get(c => c.ClaimStatus == "Submitted" || c.ClaimStatus == "In-Review");

            return Ok(pendingClaims);
        }
        [Authorize(Roles = "Administrator,ClaimProcessors")]
        // PUT: api/claims/approve/{id}
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveClaim(int id)
        {
            var claim = await _genericServiceAsync.GetOne(id);

            if (claim == null)
            {
                return NotFound();
            }

            claim.ClaimStatus = "Approved";
            
            await _genericServiceAsync.Update(claim);

            return NoContent();
        }
        [Authorize(Roles = "Administrator,ClaimProcessors")]
        // PUT: api/claims/decline/{id}
        [HttpPut("decline/{id}")]
        public async Task<IActionResult> DeclineClaim(int id)
        {
            var claim = await _genericServiceAsync.GetOne(id);

            if (claim == null)
            {
                return NotFound();
            }

            claim.ClaimStatus = "Declined";
            await _genericServiceAsync.Update(claim);

            return NoContent();
        }
        private async Task <bool> ClaimExists(int id)
        {
            
                var Claims= await _genericServiceAsync.GetOne(id);
            if(Claims is null)
            {
                return false;
            }

            return true;
        }
    }
}
