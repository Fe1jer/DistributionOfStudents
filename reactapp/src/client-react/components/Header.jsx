import SidebarMenu from "./SidebarMenu.jsx";

import { Link } from 'react-router-dom'

export default function Header() {
    return (
        <header>
            {/*Sidebar*/}
            <nav id="sidebarMenu" className="d-lg-block sidebar collapse bg-white">
                <SidebarMenu />
            </nav>
            {/*Sidebar*/}
            {/*Navbar*/}
            <nav id="main-navbar"
                className="navbar navbar-expand-lg navbar-light bg-success fixed-top shadow-sm">
                {/*Container wrapper*/}
                <div className="container-fluid">
                    {/*Toggle button*/}
                    <button className="navbar-toggler bg-light" type="button" data-bs-toggle="collapse" data-bs-target="#sidebarMenu" aria-controls="sidebarMenu" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    {/*Brand*/}
                    <Link className="navbar-brand p-0" to="/">
                        <p className="text-light font-monospace ms-3 m-0" style={{ fontSize: 'xx-large', lineHeight: 'normal' }}>БНТУ</p>
                    </Link>
                    {/*Search form*/}
                    <form asp-controller="Students" asp-action="Index" method="get" className="d-none d-sm-flex input-group w-auto my-auto">
                        <input name="searchStudents" autoComplete="off" type="search" className="form-control rounded" placeholder='Найти студента' style={{ minWidth: 300 }} />
                        <button className="input-group-text border-0" type="submit">
                            <svg xmlns="http://www.w3.org/2000/svg" height="20" fill="currentColor" className="bi bi-search" viewBox="0 0 16 16">
                                <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z" />
                            </svg>
                        </button>
                    </form>
                    {/*Right links*/}
                    <ul className="navbar-nav ms-auto d-flex flex-row">
                        {/*Avatar*/}
                        <li className="dropdown mt-2-dropdown">
                            <a className="btn p-0 d-block link-light dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                                <img src="/img/Users/bntu.jpg" alt="avatar" width="40" height="40" style={{ objectFit: 'cover', borderRadius: '50%' }} />
                            </a>
                            <ul className="dropdown-menu text-small" aria-labelledby="dropdownUser1">
                                <li><p className="dropdown-item-text text-nowrap my-0">Приветствуем <b>{/*@User.FindFirst("Name").Value.ToString()*/}</b></p></li>
                                <li><hr className="dropdown-divider" /></li>
                                <li><a className="dropdown-item" asp-controller="Account" asp-action="Profile">Профиль</a></li>
                                <li><a className="dropdown-item disabled" asp-controller="Account" asp-action="Settings" target="_blank">Настройки</a></li>
                                <li><hr className="dropdown-divider" /></li>
                                <li><a className="dropdown-item" asp-controller="Account" asp-action="Logout">Выйти</a></li>
                            </ul>
                        </li>
                    </ul>
                </div>
                {/*Container wrapper*/}
            </nav>
            {/*Navbar*/}
            <script src="/js/sidebar.js"></script>
        </header>
    );
}