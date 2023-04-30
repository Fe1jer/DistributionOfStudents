import SidebarMenu from "./SidebarMenu";
import Search from "./Searh";
import Form from 'react-bootstrap/Form';

import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom'

import "../../css/sidebar.css"

export default function Header() {
    const navigate = useNavigate();
    const [searhText, setSearhText] = useState("");

    const onSearhChange = (text) => {
        setSearhText(text);
    }
    const handleSubmit = (e) => {
        e.preventDefault();
        navigate("/Students?searchStudents=" + searhText);
    }
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
                    <Form onSubmit={handleSubmit} className="d-none d-sm-flex" style={{ minWidth: 300 }}>
                        <Search filter={onSearhChange} />
                    </Form >
                    {/*Right links*/}
                    <ul className="navbar-nav ms-auto d-flex flex-row">
                        {/*Avatar*/}
                        <li className="dropdown mt-2-dropdown">
                            <button className="btn p-0 d-block link-light dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                                <img src="/img/Users/bntu.jpg" alt="avatar" width="40" height="40" style={{ objectFit: 'cover', borderRadius: '50%' }} />
                            </button>
                            <ul className="dropdown-menu text-small" aria-labelledby="dropdownUser1">
                                <li><p className="dropdown-item-text text-nowrap my-0">Приветствуем <b>{/*@User.FindFirst("Name").Value.ToString()*/}</b></p></li>
                                <li><hr className="dropdown-divider" /></li>
                                <li><Link className="dropdown-item" to="#">Профиль</Link></li>
                                <li><Link className="dropdown-item disabled" to="#" target="_blank">Настройки</Link></li>
                                <li><hr className="dropdown-divider" /></li>
                                <li><Link className="dropdown-item" to="#">Выйти</Link></li>
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