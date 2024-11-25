using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task2
{
    public partial class Form1 : Form
    {
        private PictureBox originalPictureBox; // Початкове зображення
        private PictureBox mirroredPictureBox; // Дзеркальне зображення
        private Button selectFilesButton;      // Кнопка для вибору зображень
        private Button mirrorImageButton;      // Кнопка для дзеркального відображення
        private OpenFileDialog openFileDialog; // Діалог вибору файлу
        private string selectedFilePath;       // Шлях до обраного файлу

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Ініціалізація компонентів
            this.Text = "Image Mirror App";
            this.Width = 850;
            this.Height = 650;

            // PictureBox для оригінального зображення
            originalPictureBox = new PictureBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(20, 20),
                Size = new Size(350, 500),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // PictureBox для дзеркального зображення
            mirroredPictureBox = new PictureBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(400, 20),
                Size = new Size(350, 500),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Кнопка для вибору зображень
            selectFilesButton = new Button
            {
                Text = "Select Image",
                Font = new Font("Arial", 12),
                Location = new Point(120, 530),
                Width = 150,
                Height = 40
            };
            selectFilesButton.Click += SelectFilesButton_Click; // Прив'язка обробника події клік

            // Кнопка для дзеркального відображення
            mirrorImageButton = new Button
            {
                Text = "Mirror Image",
                Font = new Font("Arial", 12),
                Location = new Point(500, 530),
                Width = 150,
                Height = 40
            };
            mirrorImageButton.Click += MirrorImageButton_Click;

            // Діалог вибору файлу
            openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            // Додавання компонентів на форму
            this.Controls.Add(originalPictureBox);
            this.Controls.Add(mirroredPictureBox);
            this.Controls.Add(selectFilesButton);
            this.Controls.Add(mirrorImageButton);
        }

        // Обробник кнопки вибору зображення
        private void SelectFilesButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK) // Відкриваємо діалогове вікно для вибору файлів
            {
                selectedFilePath = openFileDialog.FileName; // Зберігаємо шлях до обраного фвйлу
                originalPictureBox.Image = Image.FromFile(selectedFilePath); // Показуємо початкове зображення
                mirroredPictureBox.Image = null; // Очищаємо правий PictureBox
            }
        }

        // Обробник кнопки дзеркального відображення
        private void MirrorImageButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath)) // Перевірка чи обрано файл
            {
                MessageBox.Show("Please select an image first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var originalImage = Image.FromFile(selectedFilePath))
                {
                    // Створення дзеркальної копії
                    var mirroredImage = new Bitmap(originalImage);
                    mirroredImage.RotateFlip(RotateFlipType.RotateNoneFlipX);

                    // Показуємо дзеркальне зображення
                    mirroredPictureBox.Image = mirroredImage;

                    // Додавання суфіксу -mirrored і зміна розширення на .gif
                    string directory = System.IO.Path.GetDirectoryName(selectedFilePath);
                    string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(selectedFilePath);
                    string mirroredFileName = System.IO.Path.Combine(directory, $"{fileNameWithoutExtension}-mirrored.gif");

                    // Збереження дзеркального зображення у форматі GIF
                    mirroredImage.Save(mirroredFileName, System.Drawing.Imaging.ImageFormat.Gif);

                    MessageBox.Show($"Mirrored image saved as: {mirroredFileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing the image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
