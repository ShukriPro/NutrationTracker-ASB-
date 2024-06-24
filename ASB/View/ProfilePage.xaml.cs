using System;
using System.IO;
using System.Text.Json;
using ASB.Data;
using Microsoft.Maui.Controls;

namespace ASB
{
    public partial class ProfilePage : ContentPage
    {
        private PersonalInformation personalInfo;

        public ProfilePage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                personalInfo = new PersonalInformation
                {
                    FullName = nameInput.Text,
                    Age = Convert.ToInt32(ageInput.Text),
                    Gender = genderInput.Text,
                    Weight = Convert.ToDouble(weightInput.Text),
                    Height = Convert.ToDouble(heightInput.Text),
                    MedicalConditions = medicalConditionEditor.Text
                };

                var json = JsonSerializer.Serialize(personalInfo);
                var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PersonalInformation.json");
                await File.WriteAllTextAsync(file, json);

                // Clear text fields after saving
                nameInput.Text = string.Empty;
                ageInput.Text = string.Empty;
                genderInput.Text = string.Empty;
                weightInput.Text = string.Empty;
                heightInput.Text = string.Empty;
                medicalConditionEditor.Text = string.Empty;

                await DisplayAlert("Success", "Personal information saved successfully.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
            }
        }

        private async void OnLoadClicked(object sender, EventArgs e)
        {
            try
            {
                var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PersonalInformation.json");

                if (File.Exists(file))
                {
                    var json = await File.ReadAllTextAsync(file);

                    personalInfo = JsonSerializer.Deserialize<PersonalInformation>(json);

                    // Update UI with loaded data
                    nameInput.Text = personalInfo.FullName;
                    ageInput.Text = personalInfo.Age.ToString();
                    genderInput.Text = personalInfo.Gender;
                    weightInput.Text = personalInfo.Weight.ToString();
                    heightInput.Text = personalInfo.Height.ToString();
                    medicalConditionEditor.Text = personalInfo.MedicalConditions;

                    await DisplayAlert("Success", "Personal information loaded successfully.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "PersonalInformation.json not found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load: {ex.Message}", "OK");
            }
        }
    }
}
