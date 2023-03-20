using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Threading;
using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services
{
    public class SpoolService
    {
        private readonly SpoolRepository spoolRepository;
        private readonly CommentRepository commentRepository;
        public SpoolService(PostgresDbContext context)
        {
            this.spoolRepository = new SpoolRepository(context);
            this.commentRepository = new CommentRepository(context);
        }

        public async Task<Spool> GetSpoolAsync(string spoolId)
        {
            Spool? returnedSpool = await this.spoolRepository.GetSpoolAsync(spoolId);
            if (returnedSpool != null)
            {
                return returnedSpool;
            }
            else
            {
                throw new Exception("Spool does not exist.");
            }
        }

        public async Task<Spool?> GetSpoolByNameAsync(string spoolName)
        {
            Spool? returnedSpool = await this.spoolRepository.GetSpoolByNameAsync(spoolName);
            if (returnedSpool != null)
            {
                return returnedSpool;
            }
            else
            {
                throw new Exception("Spool does not exist.");
            }
        }

        public async Task<Spool> InsertSpoolAsync(Spool spool)
        {
            string spoolName = spool.Name.Trim(' ');
            if (spool.Name.IsNullOrEmpty() || spoolName.IsNullOrEmpty() )
            {
                throw new Exception("Please enter a valid spool name.");
            }
            if (!Regex.IsMatch(spool.Name, @"^[a-zA-Z0-9]+$"))
            {
                throw new Exception("Spool name can only contain letters and numbers.");
            }
            Spool? returnedSpool = await this.spoolRepository.GetSpoolByNameAsync(spool.Name);
            if (returnedSpool != null)
            {
                throw new Exception("Spool already exists.");
            }
            else
            {
                if(spool.Name.Length > 25)
                {
                    throw new Exception("Spool name maximum is 25 characters. Please shorten name.");
                }
                await this.spoolRepository.InsertSpoolAsync(spool);
                return spool!;
            }
        }

        public async Task<Spool> AddModeratorAsync(string spoolId, string userName)
        {
            UserRepository userRepository = new UserRepository(new PostgresDbContext());

            Spool currentSpool = await this.GetSpoolAsync(spoolId);
            UserDTO spoolOwner = (await userRepository.GetUserAsync(currentSpool!.OwnerId))!;

            if(spoolOwner.Username == userName)
            {
                throw new Exception("Cannot add owner as moderator.");
            }

            UserDTO[] mods = (await this.spoolRepository.GetModeratorsAsync(spoolId))!;
            UserDTO newMod = (await userRepository.GetUserByLoginIdentifierAsync(userName))!;
            if (mods.Any(m => m.Id == newMod.Id)) {
                throw new Exception("User is already a mod.");
            }

            Spool dbSpool = (await this.spoolRepository.AddModeratorAsync(spoolId, userName))!;
            return dbSpool;
        }

        public async Task<Spool?> ChangeOwnerAsync(string spoolId, string userName)
        {
            UserRepository userRepository = new UserRepository( new PostgresDbContext() );
            Spool? currentSpool = await spoolRepository.GetSpoolAsync(spoolId);
            if(currentSpool == null)
            {
                throw new Exception("Spool does not exist");
            }
            UserDTO? currentOwner = await userRepository.GetUserAsync(currentSpool!.OwnerId);
            UserDTO? newOwner = await userRepository.GetUserByLoginIdentifierAsync(userName);

            if (currentOwner != null && currentSpool != null && newOwner != null && currentOwner.Id == newOwner.Id)
            {
                throw new Exception("User is already the owner.");
            }

            Spool dbSpool = (await this.spoolRepository.ChangeOwnerAsync(spoolId, userName))!;
            return dbSpool;
        }

        public async Task<Spool?> RemoveModeratorAsync(string spoolId, string userId)
        {
            return await this.spoolRepository.RemoveModeratorAsync(spoolId, userId);
        }

        public async Task<Spool[]> GetAllSpoolsAsync()
        {
            Spool[] spools = await this.spoolRepository.GetAllSpoolsAsync();
            return spools;
        }

        public async Task<List<Spool>> GetJoinedSpoolsAsync(string userId)
        {
            List<Spool> spools = await this.spoolRepository.GetJoinedSpoolsAsync(userId);
            return spools;
        }
        
        public async Task<List<Spool>> GetSuggestedSpoolsAsync(string userId)
        {
            List<Spool> spools = await this.spoolRepository.GetSuggestedSpoolsAsync(userId);
            return spools;
        }

        public async Task<UserDTO[]?> GetAllModsForSpoolAsync(string spoolId)
        {
            UserDTO[]? users = await this.spoolRepository.GetModeratorsAsync(spoolId);
            return users;
        }

        public async Task DeleteSpoolAsync(string spoolId, string userId)
        {
            Spool? spool = await this.spoolRepository.GetSpoolAsync(spoolId);
            if (spool == null)
            {
                throw new Exception("Spool does not exist.");
            }

            if (spool.OwnerId != userId)
            {
                throw new Exception("User does not have permission to delete spool.");
            }

            await this.commentRepository.HardDeleteAllSpoolCommentsAsync(spoolId);
            await this.spoolRepository.DeleteSpoolAsync(spoolId);
        }

        public async Task SaveRulesAsync(string spoolId, string rules)
        {
            await this.spoolRepository.SaveRulesAsync(spoolId, rules);
        }
    }
}
