using ThreaditAPI.Database;
using ThreaditAPI.Models;
using ThreaditAPI.Repositories;

namespace ThreaditAPI.Services{
    public class InterestService{
        private readonly InterestRepository interestRepository;
        public InterestService(PostgresDbContext context){
            this.interestRepository = new InterestRepository(context);
        }

        public async Task<Interest[]> GetAllInterestsAsync(){
            Interest[] interests = await this.interestRepository.GetAllInterestsAsync();
            return interests;
        }
    }
}