using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace vehicle_Rentel_Mangment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            
            bool isEligible = true;
            string errorMessage = "";

            // Input variables
            string customerName = txtName1.Text.Trim();
            int age = 0;
            int rentalDays = 0;
            int drivingExperience = 0;

            // 1. Validate Customer Name
            if (string.IsNullOrEmpty(customerName))
            {
                isEligible = false;
                errorMessage += "- Customer Name cannot be empty.\n";
            }

            // 2. Validate Customer Age using TryParse (Must be between 18 and 80)
            if (!int.TryParse(txtage.Text, out age) || age < 18 || age > 80)
            {
                isEligible = false;
                errorMessage += "- Customer age must be between 18 and 80 years.\n";
            }
            // Customers below 21 years are not eligible
            else if (age < 21)
            {
                isEligible = false;
                errorMessage += "- Customers below 21 years are not eligible to rent.\n";
            }

            // 3. Validate Rental Days using TryParse (Must be between 1 to 30 days)
            if (!int.TryParse(txtdays.Text, out rentalDays) || rentalDays < 1 || rentalDays > 30)
            {
                isEligible = false;
                errorMessage += "- Rental days must be between 1 and 30 days.\n";
            }

            // 4. Validate Driving Experience using TryParse (Cannot be negative)
            if (!int.TryParse(txtexperience.Text, out drivingExperience) || drivingExperience < 0)
            {
                isEligible = false;
                errorMessage += "- Driving experience cannot be negative.\n";
            }
            // Customers must have at least 1 year of driving experience
            else if (drivingExperience < 1)
            {
                isEligible = false;
                errorMessage += "- Customers must have at least 1 year of driving experience.\n";
            }

            // Determine Vehicle Type and Daily Rate
            string vehicleType = "";
            double dailyRate = 0;

            if (rdoeconomy.Checked)
            {
                vehicleType = "Economy";
                dailyRate = 35;
            }
            else if (rdosuv.Checked)
            {
                vehicleType = "SUV";
                dailyRate = 60;
            }
            else if (rdoluxury.Checked)
            {
                vehicleType = "Luxury";
                dailyRate = 100;

                // Luxury vehicles require at least 25 years of age and 3 years of driving experience
                if (isEligible && (age < 25 || drivingExperience < 3))
                {
                    isEligible = false;
                    errorMessage += "- Luxury vehicles require at least 25 years of age and 3 years of driving experience.\n";
                }
            }
            else
            {
                isEligible = false;
                errorMessage += "- Please select a vehicle type.\n";
            }

            // If any validation failed, display error and update status to Rejected
            if (!isEligible)
            {
                MessageBox.Show(errorMessage, "Rental Rejected", MessageBoxButtons.OK, MessageBoxIcon.Error);

                lstresults.Items.Clear();
                lstresults.Items.Add("Rental Status : REJECTED");
                lstresults.Items.Add("----------------------------------------");
                lstresults.Items.Add(errorMessage);
                return;
            }

            // --- Cost Calculations ---

            // Calculate Basic Rental Cost
            double basicRentalCost = dailyRate * rentalDays;

            // Calculate Discount based on rental days
            double discountPercent = 0;
            if (rentalDays >= 7 && rentalDays <= 13)
            {
                discountPercent = 0.05; // 5% discount
            }
            else if (rentalDays >= 14)
            {
                discountPercent = 0.10; // 10% discount
            }
            double discountAmount = basicRentalCost * discountPercent;

            // Calculate Insurance Cost ($15 per day if selected)
            double insuranceCost = 0;
            if (chkinsurance.Checked)
            {
                insuranceCost = 15 * rentalDays;
            }

            // Calculate Young Driver Surcharge (Age 21-24: Add 10% of basic rental cost)
            double surchargeAmount = 0;
            if (age >= 21 && age <= 24)
            {
                surchargeAmount = basicRentalCost * 0.10;
            }

            // Calculate Final Rental Cost
            double finalRentalCost = basicRentalCost - discountAmount + insuranceCost + surchargeAmount;

            // --- Display Rental Details and Results ---
            lstresults.Items.Clear();
            lstresults.Items.Add($"Customer Name       : {customerName}");
            lstresults.Items.Add($"Customer Age        : {age}");
            lstresults.Items.Add($"Driving Experience  : {drivingExperience} years");
            lstresults.Items.Add($"Vehicle Type        : {vehicleType}");
            lstresults.Items.Add($"Daily Rate          : ${dailyRate:F2}");
            lstresults.Items.Add($"Rental Days         : {rentalDays}");
            lstresults.Items.Add($"Basic Rental Cost   : ${basicRentalCost:F2}");
            lstresults.Items.Add($"Discount            : -${discountAmount:F2} ({discountPercent * 100}%)");
            lstresults.Items.Add($"Insurance Cost      : +${insuranceCost:F2}");
            lstresults.Items.Add($"Young Driver Surcharge: +${surchargeAmount:F2}");
            lstresults.Items.Add($"Final Rental Cost   : ${finalRentalCost:F2}");
            lstresults.Items.Add("----------------------------------------");
            lstresults.Items.Add("Rental Status       : APPROVED");
        }

       

      
        private void btnClear_Click_1(object sender, EventArgs e)
        {
            // Clear all textboxes
            txtName1.Clear();
            txtage.Clear();
            txtdays.Clear();
            txtexperience.Clear();

            // Reset radio buttons and checkbox
            rdoeconomy.Checked = false;
            rdosuv.Checked = false;
            rdoluxury.Checked = false;
            chkinsurance.Checked = false;

            // Clear the results list
            lstresults.Items.Clear();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
        

   
