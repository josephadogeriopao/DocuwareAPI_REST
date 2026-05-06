import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Log from './pages/Log';

function ProjectRoutes() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Navigate to="/logs" />} />
                <Route path="/logs" element={<Log />} />
            </Routes>
        </BrowserRouter>
    );
}

export default ProjectRoutes;