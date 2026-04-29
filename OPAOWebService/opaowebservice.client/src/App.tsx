import { useEffect, useState, useMemo } from 'react';
import './App.css';
import { DUMMY_DATA } from './data/errors';

interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

function App() {
    const [forecasts, setForecasts] = useState<Forecast[]>();

    useEffect(() => {
        populateWeatherData();
    }, []);

    const contents = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tableLabel">Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
            <br /><br /><br />
            <ErrorLogViewer />

        </div>
    );

    async function populateWeatherData() {
        const response = await fetch('weatherforecast');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }
}


const ErrorLogViewer = () => {
    const [fromDate, setFromDate] = useState('');
    const [toDate, setToDate] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 5;

    // Filter Logic
    const filteredLogs = useMemo(() => {
        return DUMMY_DATA.filter(log => {
            const logDate = new Date(log.datetime).getTime();
            const start = fromDate ? new Date(fromDate).getTime() : -Infinity;
            const end = toDate ? new Date(toDate).getTime() : Infinity;
            return logDate >= start && logDate <= end;
        });
    }, [fromDate, toDate]);

    // Pagination Logic
    const totalPages = Math.ceil(filteredLogs.length / itemsPerPage);
    const currentLogs = filteredLogs.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

    return (
        <div style={{ display: 'flex', gap: '20px', padding: '20px', fontFamily: 'Arial' }}>
            {/* Sidebar Filters */}
            <aside style={{ width: '250px', borderRight: '1px solid #ddd', paddingRight: '20px' }}>
                <h3>Search Filters</h3>
                <div style={{ marginBottom: '15px' }}>
                    <label>Date From:</label>
                    <input type="date" value={fromDate} onChange={(e) => setFromDate(e.target.value)} style={{ width: '100%' }} />
                </div>
                <div style={{ marginBottom: '15px' }}>
                    <label>Date To:</label>
                    <input type="date" value={toDate} onChange={(e) => setToDate(e.target.value)} style={{ width: '100%' }} />
                </div>
                <button onClick={() => { setFromDate(''); setToDate(''); }} style={{ width: '100%' }}>Clear Filters</button>
            </aside>

            {/* Main Table Content */}
            <main style={{ flex: 1 }}>
                <h2>Error Logs</h2>
                <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                    <thead>
                        <tr style={{ background: '#f4f4f4', textAlign: 'left' }}>
                            <th style={{ padding: '10px', border: '1px solid #ddd' }}>Date Time</th>
                            <th style={{ padding: '10px', border: '1px solid #ddd' }}>Layer/Tier</th>
                            <th style={{ padding: '10px', border: '1px solid #ddd' }}>Exception</th>
                            <th style={{ padding: '10px', border: '1px solid #ddd' }}>Description</th>
                        </tr>
                    </thead>
                    <tbody>
                        {currentLogs.map(log => (
                            <tr key={log.id}>
                                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{log.datetime.replace('T', ' ')}</td>
                                <td style={{ padding: '10px', border: '1px solid #ddd' }}><strong>{log.tier}</strong></td>
                                <td style={{ padding: '10px', border: '1px solid #ddd', color: 'red' }}>{log.exception}</td>
                                <td style={{ padding: '10px', border: '1px solid #ddd' }}>{log.description}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>

                {/* Pagination Controls */}
                <div style={{ marginTop: '20px', display: 'flex', gap: '5px' }}>
                    {Array.from({ length: totalPages }, (_, i) => (
                        <button
                            key={i + 1}
                            onClick={() => setCurrentPage(i + 1)}
                            style={{ padding: '5px 10px', backgroundColor: currentPage === i + 1 ? '#007bff' : '#fff', color: currentPage === i + 1 ? '#fff' : '#000' }}
                        >
                            {i + 1}
                        </button>
                    ))}
                </div>
            </main>
        </div>
    );
};


export default App;