using Agricultural_For_CV_BLL.Services;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_DAL.Interfaces;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Agricultural_For_CV_Tests.Services.Users
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IImageService> _ImageServiceMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly IOptions<AppSettings> _options;
        private readonly UserService _userService;


        public UserServiceTests()
        {
            //1. setup Mocks
            _userRepoMock = new Mock<IUserRepository>();
            _ImageServiceMock = new Mock<IImageService>();
            _loggerMock = new Mock<ILogger<UserService>>();

            var settings = new AppSettings { ImagePaths = new ImagePaths { Users = "path/to/users" } };

            _options = Options.Create(settings);


            //2. inj Mocks 
            _userService = new UserService(
                    _userRepoMock.Object,
                    _ImageServiceMock.Object,
                    _options,
                    _loggerMock.Object

                );

        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnSuccess_WhenUserExists()
        {

            // Arrange 
            int userId = 1;
            var fakeUser = new User { Id = userId, Username = "Hassan", Email = "Hassan@gmail.com", UserType = 1 };

            // Act 
            var result = await _userService.GetByIdAsync(userId);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Hassan", result.Data!.Username);
            _userRepoMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);

        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 99;
            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("User not found.", result.Error);
        }




        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccess_AndDeleteImage_WhenUserExists()
        {
            // Arrange 
            int userId = 1;
            string fileName = "profile_pic.jpg";
            var fakeUser = new User { Id = userId, ImageFile = fileName };

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(fakeUser);

            _userRepoMock.Setup(repo => repo.DeleteAsync(userId)).Returns(Task.CompletedTask);

            // Act 
            var result = await _userService.DeleteAsync(userId);

            // Assert 
            Assert.True(result.IsSuccess);
            Assert.Equal("User deleted successfully.", result.Message);

            // Verify if row deleted in database 
            // التأكد من استدعاء حذف السجل من قاعدة البيانات
            _userRepoMock.Verify(repo => repo.DeleteAsync(userId), Times.Once);

            // التأكد من استدعاء حذف الصورة من المجلد (هذه نقطة قوة في اختبارك)
            _ImageServiceMock.Verify(service => service.DeleteImage(fileName), Times.Once);
        }

    }

}