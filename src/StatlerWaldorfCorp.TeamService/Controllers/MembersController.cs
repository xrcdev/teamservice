using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using StatlerWaldorfCorp.TeamService.Models;
using System.Threading.Tasks;

namespace StatlerWaldorfCorp.TeamService
{
	[Route("/teams/{teamId}/[controller]")]
	public class MembersController : Controller
	{
		ITeamRepository repository;

		public MembersController(ITeamRepository repo) 
		{
			repository = repo;
		}

		[HttpGet]
		[Route("/teams/{teamId}/[controller]/members")]		
		public async virtual Task<IActionResult> GetMembers(Guid teamID) 
		{
			Team team = repository.GetTeam(teamID);
			
			if(team == null) {
				return this.NotFound();
			} else {
				return this.Ok(team.Members);
			}			
		}
		

		[HttpGet]
		[Route("/teams/{teamId}/[controller]/{memberId}")]		
		public async virtual Task<IActionResult> GetMember(Guid teamID, Guid memberId) 
		{
			Team team = repository.GetTeam(teamID);
			
			if(team == null) {
				return this.NotFound();
			} else {
				var q = team.Members.Where(m => m.ID == memberId);

				if(q.Count() < 1) {
					return this.NotFound();
				} else {
					return this.Ok(q.First());
				}				
			}			
		}

		[HttpPut]
		[Route("/teams/{teamId}/[controller]/{memberId}")]		
		public async virtual Task<IActionResult> UpdateMember([FromBody]Member updatedMember, Guid teamID, Guid memberId) 
		{
			Team team = repository.GetTeam(teamID);
			
			if(team == null) {
				return this.NotFound();
			} else {
				var q = team.Members.Where(m => m.ID == memberId);

				if(q.Count() < 1) {
					return this.NotFound();
				} else {
					team.Members.Remove(q.First());
					team.Members.Add(updatedMember);
					return this.Ok();
				}
			}			
		}

		[HttpPost]
		public async virtual Task<IActionResult> CreateMember([FromBody]Member newMember, Guid teamID) 
		{
			Team team = repository.GetTeam(teamID);
			
			if(team == null) {
				return this.NotFound();
			} else {
				team.Members.Add(newMember);
				var teamMember = new {TeamID = team.ID, MemberID = newMember.ID};
				return this.Created($"/teams/{teamMember.TeamID}/[controller]/{teamMember.MemberID}", teamMember);
			}
		}
    }
}