using EmployeeService.ClientHttp.Interfaces;
using EmployeeService.Shared.DTOs;
using System.Net.Http.Json;

namespace EmployeeService.ClientHttp.Implementations
{
    /// <summary>
    /// Implementazione del client HTTP per il servizio Employee.
    /// Utilizza HttpClient per comunicare con le API REST del microservizio.
    /// </summary>
    public class EmployeeClient : IEmployeeClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Inizializza una nuova istanza del client HTTP.
        /// </summary>
        /// <param name="httpClient">HttpClient configurato tramite IHttpClientFactory.</param>
        public EmployeeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Recupera tutti i dipendenti dal servizio remoto.
        /// </summary>
        public async Task<IEnumerable<EmployeeDto>?> GetEmployeesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeDto>>("api/employees");
        }

        /// <summary>
        /// Recupera un dipendente tramite ID.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        public async Task<EmployeeDto?> GetEmployeeByIdAsync(short id)
        {
            return await _httpClient.GetFromJsonAsync<EmployeeDto>($"api/employees/{id}");
        }

        /// <summary>
        /// Verifica l'idoneità alla guida di un dipendente.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        public async Task<bool> GetEmployeeEligibilityAsync(short id)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"api/employees/{id}/eligibility");
        }

        /// <summary>
        /// Crea un nuovo dipendente tramite API REST.
        /// </summary>
        /// <param name="dto">Dati del dipendente da creare.</param>
        /// <returns>Identificativo del dipendente creato oppure null in caso di errore HTTP.</returns>
        public async Task<short?> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/employees", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<short>();
            }

            return null;
        }
    }
}