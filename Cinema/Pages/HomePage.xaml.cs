using Cinema.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Cinema.Pages
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            List<Movie> movies = new List<Movie>
            {
                new Movie
                {
                    Name = "Погружение",
                    Rating = 8.7,
                    AgeRating = "3+",
                    Genre = "Фантастика, драма",
                    Description = "Команда исследователей отправляется на поиски нового дома для человечества.",
                    ImagePath = "/Images/faggot.jpg"
                }
             };  
            MoviesList.ItemsSource = movies;
        }
    }
}
