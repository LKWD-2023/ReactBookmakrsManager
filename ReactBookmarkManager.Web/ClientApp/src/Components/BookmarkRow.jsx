import React from 'react';

const BookmarkRow = ({ bookmark, onEditClick, onUpdateClick, onCancelClick, onDeleteClick, editMode, onTitleChange }) => {
     return (
        <tr>
            <td>
                {!editMode && bookmark.title}
                {editMode && <input type="text"
                    className="form-control"
                    onChange={onTitleChange}
                    value={bookmark.title}
                    placeholder="Title" />}
            </td>
            <td>
                <a href={bookmark.url} target="_blank">
                    {bookmark.url}
                </a>
            </td>
            <td>
                {!editMode && <button className="btn btn-success" onClick={onEditClick}>Edit Title</button>}
                {editMode &&
                    <>
                        <button className="btn btn-warning" onClick={onUpdateClick}>Update</button>
                        <button className="btn btn-info" onClick={onCancelClick}>Cancel</button>
                    </>
                }
                <button className="btn btn-danger" style={{ marginLeft: 10 }} onClick={onDeleteClick}>Delete</button>
            </td>
        </tr>
    )
}

export default BookmarkRow;