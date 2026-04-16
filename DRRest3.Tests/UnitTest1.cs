using DRRest3.Data;
using DRRest3.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DRRest3.Tests
{
    public class MusicRecordsRepositoryTests
    {
        private static MusicRecordContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MusicRecordContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new MusicRecordContext(options);
        }

        [Fact]
        public void Create_GetAll_GetById_Update_Delete_Workflow()
        {
            // Arrange
            var context = CreateInMemoryContext("CreateGetUpdateDelete");
            var repo = new MusicRecordsRepository(context);

            var record = new MusicRecord
            {
                Title = "Test Song",
                Artist = "Test Artist",
                Duration = 180,
                Year = 2020
            };

            // Act - Create
            var created = repo.Create(record);

            // Assert - Create
            Assert.True(created.Id != 0);

            // Act - GetAll
            var all = repo.GetAll();
            Assert.Single(all);

            // Act - GetById
            var fetched = repo.GetById(created.Id);
            Assert.NotNull(fetched);
            Assert.Equal("Test Song", fetched!.Title);

            // Act - Update
            var updated = new MusicRecord
            {
                Title = "Updated Song",
                Artist = "Updated Artist",
                Duration = 200,
                Year = 2021
            };

            var updateResult = repo.Update(created.Id, updated);
            Assert.NotNull(updateResult);
            Assert.Equal("Updated Song", updateResult!.Title);

            // Act - Delete
            var deleteResult = repo.Delete(created.Id);
            Assert.True(deleteResult);
            Assert.Empty(repo.GetAll());
        }

        [Fact]
        public void GetAll_Filters_By_Title_And_Artist()
        {
            // Arrange
            var context = CreateInMemoryContext("FilterTest");
            var repo = new MusicRecordsRepository(context);

            repo.Create(new MusicRecord { Title = "Hello", Artist = "Adele", Duration = 200, Year = 2015 });
            repo.Create(new MusicRecord { Title = "Hello World", Artist = "Someone", Duration = 210, Year = 2018 });
            repo.Create(new MusicRecord { Title = "Goodbye", Artist = "Adele", Duration = 190, Year = 2016 });

            // Act & Assert - filter by title
            var titleFiltered = repo.GetAll(title: "Hello");
            Assert.Equal(2, titleFiltered.Count);

            // Act & Assert - filter by artist
            var artistFiltered = repo.GetAll(artist: "Adele");
            Assert.Equal(2, artistFiltered.Count);

            // Act & Assert - filter by both
            var bothFiltered = repo.GetAll(title: "Hello", artist: "Someone");
            Assert.Single(bothFiltered);
        }
    }
}