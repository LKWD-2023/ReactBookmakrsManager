import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useAuthDataContext } from '../AuthContext';
import { Link } from 'react-router-dom';
import BookmarkRow from '../Components/BookmarkRow';
import { produce } from 'immer';

const MyBookmarks = () => {
    const { user } = useAuthDataContext();
    const [bookmarks, setBookmarks] = useState([]);
    const [editIds, setEditIds] = useState([]);

    const getBookmarks = async () => {
        const { data } = await axios.get('/api/bookmarks/getmybookmarks');
        data.forEach(bkm => bkm.originalTitle = bkm.title);
        setBookmarks(data);
    }

    useEffect(() => {
        getBookmarks();
    }, []);

    const onUpdateClick = async (title, bookmarkId) => {
        await axios.post('/api/bookmarks/updatetitle', { title, bookmarkId });
        await getBookmarks();
        setEditIds(editIds.filter(id => id !== bookmarkId));
    }

    const onDeleteClick = async bookmarkId => {
        await axios.post('/api/bookmarks/delete', { bookmarkId });
        await getBookmarks();
    }

    const onEditClick = id => {
        setEditIds([...editIds, id]);
    }

    const onTitleChange = (evt, id) => {
        const nextState = produce(bookmarks, draftBookmarks => {
            const bookmark = draftBookmarks.find(b => b.id === id);
            bookmark.title = evt.target.value;
        });
        setBookmarks(nextState);
    }

    const onCancelClick = bookmarkId => {
        const nextState = produce(bookmarks, draftBookmarks => {
            const bookmark = draftBookmarks.find(b => b.id === bookmarkId);
            bookmark.title = bookmark.originalTitle;
        });
        setBookmarks(nextState);
        setEditIds(editIds.filter(id => id !== bookmarkId));
    }

    return (
        <div style={{ marginTop: 20 }}>
            <div className="row">
                <div className="col-md-12">
                    <h1>Welcome back {user.firstName} {user.lastName}</h1>
                    <Link to='/add-bookmark' className="btn btn-primary btn-block">
                        Add Bookmark
                    </Link>
                </div>
            </div>
            <div className="row" style={{ marginTop: 20 }}>
                <table className="table table-hover table-striped table-bordered">
                    <thead>
                        <tr>
                            <th>Title</th>
                            <th>Url</th>
                            <th>Edit/Delete</th>
                        </tr>
                    </thead>
                    <tbody>
                        {bookmarks.map(b => <BookmarkRow key={b.id}
                            bookmark={b}
                            onTitleChange={e => onTitleChange(e, b.id)}
                            editMode={editIds.includes(b.id)}
                            onEditClick={() => onEditClick(b.id)}
                            onUpdateClick={() => onUpdateClick(b.title, b.id)}
                            onCancelClick={() => onCancelClick(b.id)}
                            onDeleteClick={() => onDeleteClick(b.id)}
                        />)}
                    </tbody>
                </table>
            </div>
        </div>
    )

}

export default MyBookmarks;