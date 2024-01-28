import HomePage from "./home/HomePage.jsx"

import CreateDistributionPage from "./distribution/CreateDistributionPage.jsx"

import FacultiesPage from "./faculties/FacultiesPage.jsx"
import FacultyPage from "./faculties/FacultyPage.jsx"

import FacultiesPlans from "./recruitmentPlans/FacultiesPlans.jsx"
import CreateFacultyPlansPage from "./recruitmentPlans/CreateFacultyPlansPage.jsx"
import EditFacultyPlansPage from "./recruitmentPlans/EditFacultyPlansPage.jsx"

import GroupOfSpecialityPage from "./groupsOfSpecialities/GroupOfSpecialityPage.jsx"

import ArchiveYearsPage from "./archive/ArchiveYearsPage.jsx"
import ArchiveFormsPage from "./archive/ArchiveFormsPage.jsx"
import FacultiesArchivePage from "./archive/FacultiesArchivePage.jsx"

import StudentsPage from "./students/StudentsPage.jsx"
import SubjectsPage from "./subjects/SubjectsPage.jsx"

import UsersPage from "./users/UsersPage.jsx"

import Login from "./Login.jsx"

import { Route, Routes, Outlet, useNavigate, useLocation } from 'react-router-dom'
import { history } from "../../_helpers/history.js"
import PrivateRoute from "./PrivateRoute.jsx"
import AdminRoute from "./AdminRoute.jsx"

export default function Content() {
    // init custom history object to allow navigation from 
    // anywhere in the react app (inside or outside components)
    history.navigate = useNavigate();
    history.location = useLocation();

    return (
        <Routes>
            <Route exact path="/" element={<HomePage />} />
            <Route path="/Faculties" element={<Null />} >
                <Route index element={<FacultiesPage />} />
                <Route path=":shortName" element={<Null />} >
                    <Route index element={<FacultyPage />} />
                    <Route path="RecruitmentPlans" element={<Null />} >
                        <Route path=":lastYear/Create" element={<PrivateRoute><CreateFacultyPlansPage /></PrivateRoute>} />
                        <Route path=":year/Edit" element={<PrivateRoute><EditFacultyPlansPage /></PrivateRoute>} />
                    </Route>
                    <Route path=":groupId" element={<Null />} >
                        <Route index element={<GroupOfSpecialityPage />} />
                        <Route path="Distribution/Create" element={<PrivateRoute><CreateDistributionPage /></PrivateRoute>} />
                    </Route>
                </Route>
            </Route>
            <Route path="/RecruitmentPlans" element={<FacultiesPlans />} />
            <Route path="/Students" element={<StudentsPage />} />
            <Route path="/Subjects" element={<SubjectsPage />} />
            <Route path="/Archive" element={<Null />} >
                <Route index element={<ArchiveYearsPage />} />
                <Route exact path=":year" element={<ArchiveFormsPage />} />
                <Route path=":year/:form" element={<FacultiesArchivePage />} />
            </Route>
            <Route path="/login" element={<Login />} />
            <Route path="*" element={<h2>Ресурс не найден</h2>} />
            <Route path="Admin/Users" element={<AdminRoute><UsersPage /></AdminRoute>} />
        </Routes>
    );
}

function Null() {
    return <div>
        <Outlet />
    </div >
}