using Microsoft.EntityFrameworkCore;
using PollingServer.Models.User;
using PollingServer.Models.Poll;
using PollingServer.Models.Poll.Question;
using PollingServer.Models.Poll.Answer;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata;

namespace PollingServer.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Image.Image> Images { get; protected set; }
        public DbSet<Poll.Poll> Polls { get; protected set; }
        public DbSet<User.User> Users { get; protected set; }
        public DbSet<PollAnswers> PollsAnswers { get; protected set; }
        // --------- Questions 
        public DbSet<BaseQuestion> Questions { get; protected set; }
        public DbSet<TextFieldQuestion> TextFieldQuestions { get; protected set; }
        public DbSet<SelectQuestion> SelectQuestions { get; protected set; }
        //---------- Answers
        public DbSet<BaseAnswer> Answers { get; protected set; }
        public DbSet<TextFieldAnswer> TextFieldAnswer { get; protected set; }
        public DbSet<SelectAnswer> SelectAnswer { get; protected set; }


        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) {
            Database.EnsureCreated();
        }

        public void SeedData()
        {
            if (Polls.Count() != 0)
                return;


            Users.AddRange(
                new User.User
                {
                    Nickname = "ASMcoder",
                    Password = "Qwerty123123",
                    Email = "qwerty123123@gmail.com",
                    RegistrationTime = DateTime.UtcNow,
                },
                new User.User
                {
                    Nickname = "Anonymous",
                    Password = "SuperSecretAnonymousPassword",
                    Email = "nonexistingemail",
                    RegistrationTime = DateTime.UtcNow,
                }
            );
            this.SaveChanges();

            int userId = Users.First().Id;

            Polls.AddRange(
                new Poll.Poll()
                {
                    Access = 0,
                    Type = 0,
                    OwnerId = userId,
                    Title = "First quiz ever!",
                    Description = "Welcome to our exciting quiz! This quiz is designed to test your knowledge and challenge your skills in a fun and engaging way. Whether you are a seasoned expert or just looking to learn something new, this quiz offers a variety of questions across multiple topics to keep you entertained and informed!",
                    Questions = new BaseQuestion[]{
                        new TextFieldQuestion()
                        {
                            FieldName = "First Name",
                            Description = "Input your first name",
                            FieldPlaceholder = "your first name",
                            OrderRate = 1,
                        },
                        new TextFieldQuestion()
                        {
                            FieldName = "Last Name",
                            Description = "Input your last name",
                            FieldPlaceholder = "your last name",
                            OrderRate = 1,
                        },
                        new SelectQuestion()
                        {
                            FieldName = "Sex",
                            Description = "Select your sex",
                            Options = new List<String>
                            {
                                "Male",
                                "Female",
                            }
                        }
                    }
                },
                new Poll.Poll()
                {
                    Access = 0,
                    Type = 0,
                    OwnerId = userId,
                    Title = "Science & Nature Quiz",
                    Description = "Dive into the wonders of science and nature with this thrilling quiz! Test your understanding of the natural world, from biology to astronomy, and discover fascinating facts along the way.",
                },
                new Poll.Poll()
                {
                    Access = 0,
                    Type = 0,
                    OwnerId = userId,
                    Title = "History Buff Challenge",
                    Description = "Are you a history enthusiast? Prove your knowledge of historical events, figures, and places with our History Buff Challenge. Perfect for those who love to delve into the past."
                },
                new Poll.Poll()
                {
                    Access = 0,
                    Type = 0,
                    OwnerId = userId,
                    Title = "Pop Culture Mania",
                    Description = "Stay up-to-date with the latest trends and trivia in pop culture. From movies and music to celebrities and social media, this quiz covers it all for the ultimate pop culture fan."
                },
                new Poll.Poll()
                {
                    Access = 0,
                    Type = 0,
                    OwnerId = userId,
                    Title = "Geography Genius",
                    Description = "Think you know the world? Challenge your knowledge of countries, capitals, landmarks, and more with our Geography Genius quiz. A great way to learn about the planet we live on."
                },
                new Poll.Poll()
                {
                    Access = 0,
                    Type = 0,
                    OwnerId = userId,
                    Title = "Literary Legends",
                    Description = "For the bookworms and literature lovers, this quiz will test your knowledge of famous authors, classic novels, and literary terms. Dive into the world of words and see how much you know."
                },
                new Poll.Poll()
                {
                    Access = 0,
                    Type = 0,
                    OwnerId = userId,
                    Title = "Tech Trivia",
                    Description = "Stay sharp in the ever-evolving world of technology with this quiz. Covering topics from gadgets and gizmos to groundbreaking tech innovations, this quiz is perfect for tech enthusiasts."
                },
                new Poll.Poll()
                {
                    Access = 0,
                    Type = 0,
                    OwnerId = userId,
                    Title = "Sports Spectacular",
                    Description = "Test your sports knowledge with questions about various sports, athletes, and memorable moments in sports history. Whether you're a fan of football, basketball, or any other sport, this quiz has it all."
                },
                new Poll.Poll()
                {
                    Access = 0,
                    Type = 0,
                    OwnerId = userId,
                    Title = "Music Maestro",
                    Description = "From classical compositions to modern hits, this quiz covers a wide range of musical genres and artists. Perfect for music lovers who enjoy everything from Beethoven to Beyoncé."
                },
                new Poll.Poll()
                {
                    Access = 0,
                    Type = 0,
                    OwnerId = userId,
                    Title = "Art Appreciation",
                    Description = "Explore the world of art with this quiz that covers famous paintings, artists, and art movements. Ideal for those who appreciate visual arts and want to test their knowledge."
                }
            );
            this.SaveChanges();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
