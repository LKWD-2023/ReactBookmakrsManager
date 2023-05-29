import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Home = () => {
    const [topBookmarks, setTopBookmarks] = useState([]);

    useEffect(() => {
        const getTopBookmarks = async () => {
            const { data } = await axios.get('/api/bookmarks/topfive');
            setTopBookmarks(data);
        }

        getTopBookmarks();
    }, []);

    return (
        <div>
            <h1>Welcome to the React Bookmark Application.</h1>
            <h3>Top 5 most bookmarked links</h3>
            <table className="table table-hover table-striped table-bordered">
                <thead>
                    <tr>
                        <th>Url</th>
                        <th>Count</th>
                    </tr>
                </thead>
                <tbody>
                    {topBookmarks.map(b => <tr key={b.url}>
                        <td>
                            <a href={b.url} target="_blank">{b.url}</a>
                        </td>
                        <td>
                            {b.count}
                        </td>
                    </tr>)}
                </tbody>
            </table>
        </div>
    )
}

export default Home;