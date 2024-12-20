using System;
using System.Media;
using System.Windows.Forms;

namespace Zamanlıyıcı_ve_alarm_uygulaması
{
    public partial class Form1 : Form
    {
        private int countdownTime = 0;
        private Timer timer = new Timer { Interval = 1000 };
        private int alarmTime = -1;
        private SoundPlayer soundPlayer;


        public Form1()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;

            // SoundPlayer nesnesini örneklendir
            soundPlayer = new SoundPlayer(@"C:\ses\3.wav");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Geri sayım çalışıyorsa
            if (countdownTime > 0)
            {
                countdownTime--;
                lblCountdown.Text = TimeSpan.FromSeconds(countdownTime).ToString(@"hh\:mm\:ss");
            }

            // Şu anki yerel saati al
            DateTime now = DateTime.Now;

            // Alarm zamanı kontrolü
            if (alarmTime > 0 && DateTime.TryParse(txtAlarmTimeInput.Text, out DateTime alarmDateTime))
            {
                // Eğer şu anki saat alarm saatine eşitse alarmı çalıştır
                if (now >= alarmDateTime)
                {
                    timer.Stop(); // Zamanlayıcıyı durdur
                    PlayAlarmSound(); // Alarm sesini çal
                    MessageBox.Show("Alarm zamanı geldi!", "Alarm", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Alarmı temizle (aynı alarm tekrar çalmasın)
                    alarmTime = -1;
                }
            }

            // Geri sayım sıfıra ulaştıysa
            if (countdownTime == 0)
            {
                timer.Stop();
                PlayAlarmSound();
                MessageBox.Show("Zaman doldu!", "Alarm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void PlayAlarmSound()
        {
            try
            {
                soundPlayer.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ses çalma sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBaslat_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtTimeInput.Text, out int timeInSeconds))
            {
                countdownTime = timeInSeconds;
                lblCountdown.Text = TimeSpan.FromSeconds(countdownTime).ToString(@"hh\:mm\:ss");
                timer.Start();
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir zaman girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDurdur_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            timer.Stop();
            countdownTime = 0;
            lblCountdown.Text = "00:00:00";
        }

        private void btnAlarmEkle_Click(object sender, EventArgs e)
        {
            // DateTimePicker'dan seçilen tarihi al
            DateTime selectedDateTime = dateTimePicker1.Value;

            // Seçilen tarihi txtAlarmTimeInput'a yaz
            txtAlarmTimeInput.Text = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            // Alarm zamanı kontrolü
            if (selectedDateTime > DateTime.Now)
            {
                alarmTime = (int)(selectedDateTime - DateTime.Now).TotalSeconds;

                // Alarm verisini DataGridView'a ekle
                string sesliAlarm = "Evet"; // Varsayılan olarak sesli alarm ekleniyor
                dataGridView1.Rows.Add(selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), selectedDateTime.ToString("HH:mm:ss"), sesliAlarm);

                MessageBox.Show("Alarm verisi eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Alarm zamanı geçmiş bir tarih olamaz.", "Geçersiz Zaman", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            lblCountdown.Text = "00:00:00";

            // DateTimePicker'ın formatını ayarla
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";

            // DateTimePicker değerini yerel saate ayarla
            dateTimePicker1.Value = DateTime.Now;

            // Varsayılan olarak yerel saati txtAlarmTimeInput'a yaz
            txtAlarmTimeInput.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }



        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
