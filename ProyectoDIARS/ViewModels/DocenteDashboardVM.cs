namespace ProyectoDIARS.ViewModels
{
    public class DocenteDashboardVM
    {
        public string NombreDocente { get; set; }
        public List<HorarioClaseVM> Horarios { get; set; }
        public HorarioClaseVM? HorarioSeleccionado { get; set; }
        public List<SeccionNotasVM> Secciones { get; set; }
        public SeccionNotasVM? SeccionSeleccionada { get; set; }
    }
}
