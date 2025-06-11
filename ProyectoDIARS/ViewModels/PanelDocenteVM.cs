namespace ProyectoDIARS.ViewModels
{
    public class PanelDocenteVM
    {
        public string NombreDocente { get; set; }
        public List<HorarioClaseVM> Horarios { get; set; }
        public Dictionary<string, List<AlumnoNotaVM>> AlumnosPorSeccion { get; set; }
        public string ComentarioComportamiento { get; set; }
    }
}
