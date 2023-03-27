import HomePage from "./home/HomePage.jsx"

import FacultiesPage from "./faculties/FacultiesPage.jsx"
import CreateFacultyPage from "./faculties/CreateFacultyPage.jsx"
import EditFacultyPage from "./faculties/EditFacultyPage.jsx"

import FacultiesPlans from "./recruitmentPlans/FacultiesPlans.jsx"
import CreateFacultyPlansPage from "./recruitmentPlans/CreateFacultyPlansPage.jsx"
import EditFacultyPlansPage from "./recruitmentPlans/EditFacultyPlansPage.jsx"

import ArchiveYearsPage from "./archive/ArchiveYearsPage.jsx"
import ArchiveFormsPage from "./archive/ArchiveFormsPage.jsx"
import FacultiesArchivePage from "./archive/FacultiesArchivePage.jsx"

import StudentsPage from "./students/StudentsPage.jsx"
import SubjectsPage from "./subjects/SubjectsPage.jsx"

import { Route, Routes, Outlet } from 'react-router-dom'

export default function Content() {
    return (
        <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/Faculties" element={<Null />} >
                <Route index element={<FacultiesPage />} />
                {/*<Route path=":shortName" element={<Faculty />} />*/}
                <Route path="Create" element={<CreateFacultyPage />} />
                <Route path=":shortName/Edit" element={<EditFacultyPage />} />
                <Route path=":facultyShortName/RecruitmentPlans" element={<Null />} >
                    <Route path=":lastYear/Create" element={<CreateFacultyPlansPage />} />
                    <Route path=":year/Edit" element={<EditFacultyPlansPage />} />
                </Route>
            </Route>
            <Route path="/RecruitmentPlans" element={<Null />}>
                <Route index element={<FacultiesPlans />} />
            </Route>
            <Route path="/Students" element={<StudentsPage />} >
            </Route>
            <Route path="/Subjects" element={<SubjectsPage />} />
            <Route path="/Archive" element={<Null />} >
                <Route index element={<ArchiveYearsPage />} />
                <Route path=":year" element={<ArchiveFormsPage />} />
                <Route path=":year/:form" element={<FacultiesArchivePage />} />
            </Route>
            <Route path="*" element={<h2>Ресурс не найден</h2>} />
        </Routes>
    );
}

function Null() {
    return <div>
        <Outlet />
    </div >
}