import Container from 'react-bootstrap/Container';
import Dropdown from 'react-bootstrap/Dropdown';
import Form from 'react-bootstrap/Form';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavLink from 'react-bootstrap/NavLink';

import { LinkContainer } from 'react-router-bootstrap';
import Search from "./Searh";
import SidebarMenu from "./SidebarMenu";
import ModalWindowProfile from './users/ModalWindows/ModalWindowProfile';

import { authActions } from '../../_store';

import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import "../../css/sidebar.css";

export default function Header() {
    const authUser = useSelector(x => x.auth.user);
    const dispatch = useDispatch();
    const logout = () => dispatch(authActions.logout());
    const navigate = useNavigate();
    const [searhText, setSearhText] = useState("");
    const [profileShow, setProfileShow] = useState(false);

    const handleProfileClose = () => {
        setProfileShow(false);
    };
    const onClickShowProfileUser = () => {
        setProfileShow(true);
    }

    const _showAuthLink = () => {
        if (!authUser) {
            return <LinkContainer to="/login">
                <Nav.Link className="text-light p-0">Войти</Nav.Link>
            </LinkContainer>
        }
        else {
            return <Dropdown>
                <ModalWindowProfile show={profileShow} handleClose={handleProfileClose} />
                <Dropdown.Toggle className="text-light p-0" as={NavLink} data-bs-toggle="dropdown" >
                    <img src={authUser.img} alt="avatar" width="40" height="40" style={{ borderRadius: '50%' }} />
                </Dropdown.Toggle>
                <Dropdown.Menu renderOnMount className="text-small">
                    <Dropdown.ItemText className="d-inline-flex">Приветствуем<b className="ps-1">{authUser.name}</b></Dropdown.ItemText>
                    <Dropdown.Divider />
                    <Dropdown.Item onClick={() => onClickShowProfileUser()}>Профиль</Dropdown.Item>
                    <LinkContainer to="#">
                        <Dropdown.Item>Настройки</Dropdown.Item>
                    </LinkContainer>
                    <Dropdown.Divider />
                    <Dropdown.Item onClick={logout}>Выйти</Dropdown.Item>
                </Dropdown.Menu>
            </Dropdown>
        }
    }

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
            <Nav id="sidebarMenu" className="d-lg-block sidebar collapse bg-white">
                <SidebarMenu />
            </Nav>
            {/*Sidebar*/}
            {/*Navbar*/}
            <Navbar className="fixed-top shadow-sm" expand="lg" variant="light" bg="success">
                {/*Container wrapper*/}
                <Container fluid>
                    {/*Toggle button*/}
                    <Navbar.Toggle className="bg-light" data-bs-toggle="collapse" data-bs-target="#sidebarMenu" aria-controls="sidebarMenu" aria-expanded="false" aria-label="Toggle navigation" />
                    {/*Brand*/}
                    <LinkContainer to="/">
                        <Navbar.Brand className="p-0"><p className="text-light font-monospace ms-3 m-0" style={{ fontSize: 'xx-large', lineHeight: 'normal' }}>БНТУ</p></Navbar.Brand>
                    </LinkContainer>
                    <Nav className="me-auto">
                        {/*Search form*/}
                        <Form onSubmit={handleSubmit} className="d-none d-sm-flex" style={{ minWidth: 330 }}>
                            <Search filter={onSearhChange} />
                        </Form >
                    </Nav>
                    {_showAuthLink()}
                </Container>
                {/*Container wrapper*/}
            </Navbar>
            {/*Navbar*/}
            <script src="/js/sidebar.js"></script>

        </header>
    );
}