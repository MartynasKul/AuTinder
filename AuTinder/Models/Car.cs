namespace AuTinder.Models
{
    public class Car
    {
        public string Make {  get; set; }
        public string Model {  get; set; }
        public string BodyType { get; set; }
        public DateTime Year { get; set; } 
        public string FuelType {  get; set; }
        public int Mileage {  get; set; }
        public string Color {  get; set; }
        public DateTime Inspection {  get; set; } // Technine apziura
        public string DriveWheels {  get; set; }
        public string Gearbox { get; set; }
        public int Power {  get; set; }
        public string SteeringWheelLocation {  get; set; }
        public string OutsideState {  get; set; }
        public string ExtraFunc {  get; set; }
        public float Rating {  get; set; }

        #region Constructors
        public Car() { }

        public Car(string make, string model, string bodyType, DateTime year, string fuelType, 
            int mileage, string color, DateTime inspection, string driveWheels, string gearbox,
            int power, string steeringWheelLocation, string outsideState, string extraFunc, float rating )
        {
            Make = make;
            Model = model;
            BodyType = bodyType;
            Year = year;
            FuelType = fuelType;
            Mileage = mileage;
            Color = color;
            Inspection = inspection;
            DriveWheels = driveWheels;
            Gearbox = gearbox;
            Power = power;
            SteeringWheelLocation = steeringWheelLocation;
            OutsideState = outsideState;
            ExtraFunc = extraFunc;
            Rating = rating;
        }
        #endregion
    }
}
