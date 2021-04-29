using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;


namespace EkranAlıntısıAracı
{
    public partial class Form1 : Form
    {

        // ******************************************************** Developed by caner24 ********************************************************************* //

        public Form1()
        {
            InitializeComponent();
        }
        // Aldığımız ekran görüntüsünde herhangi bir çizme işlemi yapmak istersek diye çizme işlemi tanımlandı.
        bool cizmeislemi;
        int xkonum;  // çizme işlemini yapabilmemiz için x değişkeni oluşturuldu.
        int ykonum;  // çizme işlemini yapabilmemiz için y değişkeni oluşturuldu.
        int durum;  // çizme işlemini yapabilmemiz için x değişkeni oluşturuldu.
        int kalinlik=5; // Çizme işlemi olduğu için kalem kalınlığı başlangıç değerini atadik.
        Pen kalem; // Kalem nesnemizi oluşturduk 
        Graphics gr; // Grafik nesnemizi oluşturduk

        private void resmiKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Çalıştırdığımız programı ekran görüntüsü alırken dışında tutması için hide özelliğinden yaralandık.
            this.Hide();
            // Bitmap nesnesi oluşturuldu, genişlik ve yüksekliği bilgisayar ekranımıza göre almasını sağladık.
            Bitmap crp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            // grafik nesnemize işlem yapabilmesi için bitmap nesnemizi aktardık.
            gr = Graphics.FromImage(crp);
            // Grafik nesnemiz boyutları belirlenmiş olan görüntüyü almasını sağladık.
            gr.CopyFromScreen(0, 0, 0, 0, new Size(crp.Width, crp.Height));
            // Formumuza aldığımız ekran görüntüsünün görünmesi için BackgroundImage ' a aktardık.
            panel1.BackgroundImage = crp;
            // Tekrardan uygulamamızın görünmesiiçin show özelliğinden yararlandık.
            this.Show();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Kullanici aldığı ekran görüntüsünü kaydetmek isterse gerekli işlemler yapıldı.

            // Kaydederken çıkan açıklama
            saveFileDialog1.Title = "Kayit Edin";
            // Kayıt edeceğimiz formatlar 
            saveFileDialog1.Filter = "Resim Dosyasi(*.jpg) | *.jpg";
            // Kullanıcı kaydet ' e basarsa olacak adımlar.
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // kaydedeceğimiz dosyanın yolunu ve adresini string değişkenimize aktarıyoruz.
                string dosyaadi = saveFileDialog1.FileName;
                // Bitmap nesnesi oluşturuldu, genişlik ve yüksekliği bilgisayar ekranımıza göre almasını sağladık.
                Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
                // grafik nesnemize işlem yapabilmesi için bitmap nesnemizi aktardık.
                gr = Graphics.FromImage(bmp);
                // Belirli bir alanın görüntüsünü alacığımz için Rectangle özelliklerinden yaralandık.
                var cerceve = panel1.RectangleToScreen(panel1.ClientRectangle);
                // Grafik nesnemiz boyutları belirlenmiş olan görüntüyü almasını sağladık.
                gr.CopyFromScreen(cerceve.Location, Point.Empty, panel1.Size);
                // Aldığımız yol sayesinde resmimizi ilgili yola kaydediyoruz.
                bmp.Save(dosyaadi);
            }
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // Kalem sekmesine tıklandığı zaman ve mause hareket ediyorsa Çizme işlemini aktif ediyoruz.
            cizmeislemi = true;
            // Aktif konumları yani çizme işlemini yaparken mause imlecimizin sağa sola nereye gittiğini programın anlaması için e özelliğinden yararlandık.
            xkonum = e.X;
            ykonum = e.Y;
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            // Eğer mause basılı tutmuyorsak çizme işleminin olmaması için mouse up özelliğinden yararlanıp çizme işlemini false yaptık.
            cizmeislemi = false;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            // Eğer kalem sekmemize tıklanmışsa yani durum değişkenimize değer atanmışsa çizme işlemlerini yaptık.
            if (durum == 1)
            {
                // Grafik nesnemizi oluşturduk.
                gr = panel1.CreateGraphics();
                //Grafik nesnemizin konumlarını belirledik.
                Point konum1 = new Point(xkonum, ykonum);
                Point konum2 = new Point(e.X, e.Y);
                if (cizmeislemi == true)
                {
                    gr.DrawLine(kalem, konum1, konum2); // Kalem için Drawline özelliğimizden yararlandık böylece çizgi çizebileceğiz.
                    xkonum = e.X; // sağa sola çizgi yapılırsa xkonum ve ykonum belirleniyor yani boyut ne kadar olacığı burada yazmasan başlangıç noktası etrafında uzun devam eder.
                    ykonum = e.Y; // sağa sola çizgi yapılırsa xkonum ve ykonum belirleniyor.
                }
            }
        }
        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Eğer kalem sekmesine tıklanırsa çizme işleminin olması için durum değişkeni değeri değiştirildi.
            durum = 1;
            // Eğer kalem rengi değiştirilmek istenirse color dialog nesnemiz yaratıldı.
            ColorDialog renk = new ColorDialog();
            renk.ShowDialog();
            kalem = new Pen(renk.Color,kalinlik);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            //Kalemimizin kalınlıklarını ilgili sekmelere tıklanınca ayarlamış olduk.
            kalinlik = 3;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            //Kalemimizin kalınlıklarını ilgili sekmelere tıklanınca ayarlamış olduk.
            kalinlik = 5;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            //Kalemimizin kalınlıklarını ilgili sekmelere tıklanınca ayarlamış olduk.
            kalinlik = 8;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            //Kalemimizin kalınlıklarını ilgili sekmelere tıklanınca ayarlamış olduk.
            kalinlik = 10;
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            //Kalemimizin kalınlıklarını ilgili sekmelere tıklanınca ayarlamış olduk.
            kalinlik = 20;
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            //Kalemimizin kalınlıklarını ilgili sekmelere tıklanınca ayarlamış olduk.
            kalinlik = 30;
        }
    }
}
